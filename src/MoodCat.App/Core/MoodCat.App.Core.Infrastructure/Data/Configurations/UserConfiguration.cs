using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.Infrastructure.Data.Configurations;

/// <summary>
/// Konfiguracja encji użytkownika
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.AssignedSpecialistId);
    }
}