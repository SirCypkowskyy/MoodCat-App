using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.Questions;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja encji pytania
/// </summary>
public class QuestionEntityConfiguration : IEntityTypeConfiguration<QuestionEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        builder.ToTable("Questions");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}