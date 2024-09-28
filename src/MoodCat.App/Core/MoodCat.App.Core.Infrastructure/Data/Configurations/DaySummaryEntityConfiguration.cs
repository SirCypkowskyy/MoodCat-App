using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.DaySummaries;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja encji podsumowania dnia
/// </summary>
public class DaySummaryEntityConfiguration : IEntityTypeConfiguration<DaySummaryEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DaySummaryEntity> builder)
    {
        builder.ToTable("DaysSummaries");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Content).IsRequired();
        
        builder.Property(x => x.OriginalContent).IsRequired();
        
        builder.Ignore(x => x.OriginalContent); 
        
        builder.HasOne<User>()
            .WithMany(e => e.DaySummaries)
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}