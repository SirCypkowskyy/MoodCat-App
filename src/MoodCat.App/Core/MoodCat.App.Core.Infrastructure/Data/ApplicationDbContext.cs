using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoodCat.App.Core.Domain.DaySummaries;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Users;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Infrastructure.Data;

/// <summary>
/// Kontekst bazy danych 
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
    /// <inheritdoc />
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();
    
    /// <inheritdoc />
    public DbSet<NoteAttachment> NoteAttachments => Set<NoteAttachment>();

    /// <inheritdoc />
    public DbSet<DaySummaryEntity> DaysSummaries => Set<DaySummaryEntity>();

    /// <inheritdoc />
    public DbSet<User> Users => Set<User>();
    
    /// <summary>
    /// Konstruktor kontekstu bazy danych
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}