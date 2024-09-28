using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja encji notatki
/// </summary>
public class NoteEntityConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        builder.ToTable("Notes");
        
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).HasConversion(
            id => id.Value,
            value => NoteId.Of(value)
        );

        builder.ComplexProperty(n => n.Title, titleBuilder =>
        {
            titleBuilder.Property(n => n.Value)
                .HasColumnName(nameof(NoteTitle))
                .HasMaxLength(NoteTitle.DefaultMaxLength)
                .IsRequired();
        });
        
        builder.ComplexProperty(n => n.Content, contnetBuilder =>
        {
            contnetBuilder.Property(n => n.Value)
                .HasColumnName(nameof(NoteContent))
                .HasMaxLength(NoteContent.DefaultMaxLength)
                .IsRequired();
        });
        
        // builder.Property(c => c.Title)
        //     .IsRequired()
        //     .HasConversion(
        //         title => title.Value,
        //         value => NoteTitle.Of(value)
        //     )
        //     .HasMaxLength(NoteTitle.DefaultMaxLength);
        //
        // builder.Property(c => c.Content)
        //     .IsRequired()
        //     .HasConversion(
        //         content => content.Value,
        //         value => NoteContent.Of(value)
        //     )
        //     .HasMaxLength(NoteContent.DefaultMaxLength);

        // Relacja twórcy do notatki
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        // Relacja specjalisty do notatki 
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.AllowedNoteSupervisorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}