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
                          .OnDelete(DeleteBehavior.Cascade)
            );
    }
}
