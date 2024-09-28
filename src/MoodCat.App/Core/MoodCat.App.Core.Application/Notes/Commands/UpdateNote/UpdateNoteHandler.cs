using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Exceptions;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.Interfaces;
using MoodCat.App.Core.Application.OpenAI.Commands.SendWhisperAudioFile;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Commands.UpdateNote;

/// <summary>
/// Handler obsługi komendy od aktualizacji notatki
/// </summary>
public class UpdateNoteHandler(IApplicationDbContext dbContext, IWhisperService whisperService)
    : ICommandHandler<UpdateNoteCommand, UpdateNoteResult>
{
    /// <inheritdoc />
    public async Task<UpdateNoteResult> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        var noteId = NoteId.Of(command.NoteId);
        // Sprawdzenie, czy notatka istnieje
        var note = await dbContext.Notes
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == noteId, cancellationToken);

        if (note == null)
            throw new NotFoundException(nameof(NoteEntity), noteId);

        if (note.UserId != command.UserId)
            throw new UnauthorizedAccessException("You are not authorized to update this note.");

        var noteTitle = string.IsNullOrWhiteSpace(command.Data.Title) ? note.Title.Value : command.Data.Title;
        var noteText = string.IsNullOrWhiteSpace(command.Data.Text) ? note.Content.Value : command.Data.Text;
        var wasAudio = false;
        
        // Jeśli content się nie zmienił, ale zmieniła się 
        if (!string.IsNullOrEmpty(command.Data.AudioUrl) && noteText == note.Content.Value)
        {
            wasAudio = true;
            var transcribeAudioCmd = new SendWhisperAudioFileCommand(new WhisperRequestDTO(command.Data.AudioUrl));
            var transcribedAudio = await whisperService.TranscribeAudio(transcribeAudioCmd, cancellationToken);
            noteText = transcribedAudio.Text;
            note.AddAttachment(NoteAttachment.Of(command.Data.Title + "_Audio_Attachment", 0, command.Data.AudioUrl));
        }

        note.Update(NoteTitle.Of(noteTitle), NoteContent.Of(noteText));
        
        if(!wasAudio)
            note.RemoveAttachment(note.Attachments[0]);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateNoteResult(
            new CreateNoteResponseDTO(
                note.Id.Value.ToString(),
                note.Title.Value,
                note.Content.Value.ToString()
            )
        );
    }
}