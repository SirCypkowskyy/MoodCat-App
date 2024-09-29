using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja encji pytania do notatki
/// </summary>
public class NoteQuestionEntityConfiguration : IEntityTypeConfiguration<NoteQuestionEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NoteQuestionEntity> builder)
    {
        builder.ToTable("NotesQuestions");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Question)
            .HasMaxLength(NoteQuestionEntity.DefaultMaxQuestionLength);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<NoteQuestionEntity>(n => n.UserId);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<NoteQuestionEntity>(n => n.SpecialistId);
    }
}