using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja załącznika notatki
/// </summary>
public class NoteAttachmentConfiguration : IEntityTypeConfiguration<NoteAttachment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NoteAttachment> builder)
    {
        builder.ToTable("NotesAttachments");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).HasConversion(
            id => id.Value,
            value => NoteAttachmentId.Of(value)
        );

        builder.Property(c => c.Name)
            .HasMaxLength(NoteAttachment.MaxNameLength).IsRequired();

        builder.Property(c => c.Size);

        builder.Property(c => c.ResourceUrl)
            .HasMaxLength(NoteAttachment.MaxResourceUrlLength).IsRequired();
    }
}