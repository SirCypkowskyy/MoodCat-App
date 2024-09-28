using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.Interfaces;
using MoodCat.App.Core.Application.OpenAI.Commands.SendWhisperAudioFile;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Commands.CreateNoteAudio;

/// <summary>
/// Handler żądania stworzenia notatki z audio
/// </summary>
public class CreateNoteAudioHandler(IApplicationDbContext dbContext, IWhisperService whisperService)
    : ICommandHandler<CreateNoteAudioCommand, CreateNoteAudioResult>
{
    /// <inheritdoc />
    public async Task<CreateNoteAudioResult> Handle(CreateNoteAudioCommand command, CancellationToken cancellationToken)
    {
        var noteContentFromAudio = await GetTextFromAudioFileUrl(command.AudioUrl, cancellationToken);

        string noteContent;

        if (!string.IsNullOrWhiteSpace(command.ProvidedQuestion))
            noteContent = "Question: " + command.ProvidedQuestion + "\n\n" + "Answer: " + noteContentFromAudio;
        else
            noteContent = noteContentFromAudio;

        var note = NoteEntity.Create(
            command.UserId,
            NoteTitle.Of(command.NoteTitle),
            NoteContent.Of(noteContent),
            command.HappinessScore
        );
        
        var usr = (await dbContext.Users.FindAsync(new object?[] {command.UserId}, cancellationToken))!;
        
        if(usr.AssignedSpecialistId != null)
            note.SetAllowedNoteSupervisorId(usr.AssignedSpecialistId);

        var datetime = DateTime.Now;
        note.AddAttachment(NoteAttachment.Of($"Audio_Attachment_({note.Title.Value})_{datetime}", 0, command.AudioUrl));

        await dbContext.Notes.AddAsync(note, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateNoteAudioResult(
            new CreateNoteResponseDTO(
                note.Id.Value.ToString(),
                note.Title.Value,
                note.Content.Value.ToString()
            )
        );
    }

    private async Task<string> GetTextFromAudioFileUrl(string requestAudioUrl, CancellationToken cancellationToken)
    {
        var audioTranscription = await whisperService.TranscribeAudio(
            new SendWhisperAudioFileCommand(
                new WhisperRequestDTO(requestAudioUrl)
            ), cancellationToken
        );

        return audioTranscription.Text;
    }
}