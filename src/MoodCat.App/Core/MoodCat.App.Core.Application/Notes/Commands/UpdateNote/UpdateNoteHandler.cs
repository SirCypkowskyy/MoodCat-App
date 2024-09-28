using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Exceptions;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Commands.UpdateNote;

/// <summary>
/// Handler obsługi komendy od aktualizacji notatki
/// </summary>
public class UpdateNoteHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateNoteCommand, UpdateNoteResult>
{
    /// <inheritdoc />
    public async Task<UpdateNoteResult> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        var noteId = NoteId.Of(command.NoteId);
        // Sprawdzenie, czy notatka istnieje
        var note = await dbContext.Notes
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Id == noteId, cancellationToken);
        
        if(note == null)
            throw new NotFoundException(nameof(NoteEntity), noteId);
        
        if(note.UserId != command.UserId)
            throw new UnauthorizedAccessException("You are not authorized to update this note.");
        
        var noteTitle = string.IsNullOrWhiteSpace(command.Data.Title) ? note.Title.Value : command.Data.Title;
        var noteText = string.IsNullOrWhiteSpace(command.Data.Text) ? note.Content.Value : command.Data.Text;
        
        // Jeśli content się nie zmienił, ale zmieniła się 
        // if(!string.IsNullOrEmpty(command.Data.AudioUrl))

        throw new NotImplementedException();
    }
}