using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Exceptions;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Queries.GetNoteById;

/// <summary>
/// Handler zapytania pobrania szczegółowej notatki po ID
/// </summary>
public class GetNoteByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetNoteByIdQuery, GetNoteByIdResult>
{
    /// <inheritdoc />
    public async Task<GetNoteByIdResult> Handle(GetNoteByIdQuery query, CancellationToken cancellationToken)
    {
        var convertedQueryNoteId = NoteId.Of(query.NoteId);
        var note = await dbContext.Notes
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == convertedQueryNoteId, cancellationToken);
        
        if (note == null)
            throw new NotFoundException(nameof(NoteEntity), query.NoteId);

        var noteOwner = (await dbContext.Users.FirstOrDefaultAsync(x => x.Id == note.UserId, cancellationToken))!;
        
        if (note.UserId != query.UserId && noteOwner.AssignedSpecialistId != query.UserId)
            throw new UnauthorizedAccessException("You are not authorized to view this note.");
        
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

        GetNoteByIdResult response;

        if (note.Attachments.Any())
            response = new GetNoteByIdResult(
                new NoteDetailedResponseDTO(
                    note.Id.Value.ToString(),
                    note.Content.Value,
                    note.Title.Value,
                    note.CreatedAt ?? DateTime.Now,
                    note.LastModifiedAt,
                    note.Happiness ?? 0,
                    noteAttachments
                )
            );
        else
            response = new GetNoteByIdResult(
                new NoteDetailedResponseDTO(
                    note.Id.Value.ToString(),
                    note.Content.Value,
                    note.Title.Value,
                    note.CreatedAt ?? DateTime.Now,
                    note.LastModifiedAt,
                    note.Happiness ?? 0,
                    []
                )
            );

        return response;
    }
}