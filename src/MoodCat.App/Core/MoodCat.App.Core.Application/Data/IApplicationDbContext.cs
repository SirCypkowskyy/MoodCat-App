using Microsoft.EntityFrameworkCore;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.Core.Application.Data;

public interface IApplicationDbContext
{
    /// <summary>
    /// Użytkownicy aplikacji
    /// </summary>
    DbSet<User> Users { get; }
    
    /// <summary>
    /// Notatki w aplikacji
    /// </summary>
    DbSet<NoteEntity> Notes { get; }
    
    /// <summary>
    /// Załączniki do notatek
    /// </summary>
    DbSet<NoteAttachment> NoteAttachments { get; }
    
    /// <summary>
    /// Zapisuje zmiany w kontekście bazy danych
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Ilość rekordów, która uległa modyfikajci
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}