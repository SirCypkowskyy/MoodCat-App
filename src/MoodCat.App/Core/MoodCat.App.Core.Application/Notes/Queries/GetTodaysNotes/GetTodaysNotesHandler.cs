using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.Notes.Queries.GetNoteById;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Queries.GetTodaysNotes;

/// <summary>
/// Handler żądania pobrania dzisiejszych notatek użytkownika
/// </summary>
public class GetTodaysNotesHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTodaysNotesQuery, GetTodaysNotesResult>
{
    /// <inheritdoc />
    public async Task<GetTodaysNotesResult> Handle(GetTodaysNotesQuery request, CancellationToken cancellationToken)
    {
        var notes = dbContext.Notes
            .Include(x => x.Attachments)
            .Where(x => (x.UserId == request.UserId || x.AllowedNoteSupervisorId == request.UserId)
                        && DateOnly.FromDateTime(x.CreatedAt ?? DateTime.Now) == DateOnly.FromDateTime(DateTime.Now))
            .AsNoTracking();

        var convertedNotes = new List<NoteDetailedResponseDTO>();

        foreach (var note in notes)
        {
            var noteAttachments = Array.Empty<NoteAttachmentDetailedResponseDTO>();

            if (note.Attachments.Any())
                noteAttachments = note.Attachments.ToArray().Select(x =>
                    new NoteAttachmentDetailedResponseDTO(
                        x.Id.ToString(),
                        x.Name,
                        x.ResourceUrl,
                        x.CreatedAt ?? DateTime.Now,
                        x.LastModifiedAt)
                ).ToArray();

            NoteDetailedResponseDTO response;

            if (note.Attachments.Any())
                response =
                    new NoteDetailedResponseDTO(
                        note.Id.Value.ToString(),
                        note.Content.Value,
                        note.Title.Value,
                        note.CreatedAt ?? DateTime.Now,
                        note.LastModifiedAt,
                        note.Happiness ?? 0,
                        noteAttachments
                    );
            else
                response =
                    new NoteDetailedResponseDTO(
                        note.Id.Value.ToString(),
                        note.Content.Value,
                        note.Title.Value,
                        note.CreatedAt ?? DateTime.Now,
                        note.LastModifiedAt,
                        note.Happiness ?? 0,
                        []
                    );
            convertedNotes.Add(response);
        }

        return new GetTodaysNotesResult(convertedNotes.ToArray());
    }
}