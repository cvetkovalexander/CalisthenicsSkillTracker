using CalisthenicsSkillTracker.Models;
using CalisthenicsSkillTracker.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configuration;

public class ExerciseEntityTypeConfiguration : IEntityTypeConfiguration<Exercise>
{
    private readonly Exercise[] _exercises =
    {
         new Exercise
         {
            Id = Guid.NewGuid(),
            Name = "Pike Push-up",
            Description = "A vertical pushing exercise progressing towards a handstand push-up",
            Difficulty = Difficulty.Beginner,
            SkillId = Guid.Parse("433D8F6C-40DF-4A94-633D-08DE6290695F"),
            ExerciseType = SkillType.Push,
            Category = Category.Strength
         },
        new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "L-Sit Raises",
            Description = "Core-focused exercise improving L-sit strength and control",
            Difficulty = Difficulty.Beginner,
            SkillId = Guid.Parse("6CC6E3EB-D9F6-47C9-6B9B-08DE60D0EC0E"),
            ExerciseType = SkillType.Core,
            Category = Category.Strength
        },
        new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Tuck Front Lever Pull-up",
            Description = "Pulling exercise progressing towards full front lever",
            Difficulty = Difficulty.Beginner,
            SkillId = Guid.Parse("FF8AD0B0-6B1A-47D5-B2D5-A6DF0E59B54C"),
            ExerciseType = SkillType.Pull,
            Category = Category.Strength
        },
        new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Dip",
            Description = "Vertical pushing exercise targeting chest and triceps",
            Difficulty = Difficulty.Beginner,
            SkillId = null,
            ExerciseType = SkillType.Push,
            Category = Category.Strength
        }
    };
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        //builder
        //    .HasData(_exercises);
    }
}
