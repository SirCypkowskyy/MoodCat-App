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
    public async Task<CreateNoteAudioResult> Handle(CreateNoteAudioCommand request, CancellationToken cancellationToken)
    {
        var noteContentFromAudio = await GetTextFromAudioFileUrl(request.AudioUrl, cancellationToken);

        var note = NoteEntity.Create(request.UserId, NoteTitle.Of(request.NoteTitle),
            NoteContent.Of(noteContentFromAudio), null);
        note.AddAttachment(NoteAttachment.Of($"Audio_Attachment_({note.Title.Value})", 0, request.AudioUrl));

        await dbContext.Notes.AddAsync(note, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateNoteAudioResult(new CreateNoteResponseDTO(note.Id.Value.ToString(), note.Title.Value,
            note.Content.Value.ToString()));
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