using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder
            .HasMany(e => e.Skills)
            .WithMany(s => s.Exercises)
                .UsingEntity<Dictionary<string, object>>(
                    "ExerciseSkills",
                    j => j.HasOne<Skill>()
                          .WithMany()
                          .HasForeignKey("SkillId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>()
                          .WithMany()
                          .HasForeignKey("ExerciseId")
                          .OnDelete(DeleteBehavior.Cascade),

                    j =>
                    {
                        j.HasKey("ExerciseId", "SkillId");

                        j.HasData(
                        new Dictionary<string, object>
                        {
                            ["ExerciseId"] = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                            ["SkillId"] = Guid.Parse("11111111-1111-1111-1111-111111111111")
                        },
                        new Dictionary<string, object>
                        {
                            ["ExerciseId"] = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                            ["SkillId"] = Guid.Parse("11111111-1111-1111-1111-111111111111")
                        },
                        new Dictionary<string, object>
                        {
                            ["ExerciseId"] = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                            ["SkillId"] = Guid.Parse("55555555-5555-5555-5555-555555555555")
                        }
                    );
                });
    }
}
