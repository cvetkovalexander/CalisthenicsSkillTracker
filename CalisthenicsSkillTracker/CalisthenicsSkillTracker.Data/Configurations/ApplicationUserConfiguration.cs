using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder) 
    {
        builder
            .HasMany(u => u.FavoriteExercises)
            .WithMany(e => e.FavoritedByUsers)
            .UsingEntity<Dictionary<string, object>>(
                "UserFavoriteExercises",
                j => j
                    .HasOne<Exercise>()
                    .WithMany()
                    .HasForeignKey("ExerciseId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("UserId", "ExerciseId");
                });

        builder
            .HasMany(u => u.FavoriteSkills)
            .WithMany(s => s.FavoritedByUsers)
            .UsingEntity<Dictionary<string, object>>(
                "UserFavoriteSkills",
                j => j
                    .HasOne<Skill>()
                    .WithMany()
                    .HasForeignKey("SkillId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("UserId", "SkillId");
                });
    }
}
