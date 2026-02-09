using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CalisthenicsSkillTracker.Data.Configuration;

public class SkillEntityTypeConfiguration : IEntityTypeConfiguration<Skill>
{
    private readonly Skill[] _skills =
    {
            new Skill
            {
                Id = Guid.Parse("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"),
                Name = "Planche",
                Description = "A static hold balancing on hands with body parallel to ground",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Push
            },
            new Skill
            {
                Id = Guid.Parse("dcf2edd8-602f-4b96-8521-57b34c5ece9f"),
                Name = "Handstand",
                Description = "Balance on hands, can be against a wall or freestanding",
                MeasurementType = Measurement.Duration,
                Category = Category.Balance,
                SkillType = SkillType.Push
            },
            new Skill
            {
                Id = Guid.Parse("9aa10e18-49f5-4b90-8613-74eec06259c0"),
                Name = "Pull-Up",
                Description = "Pull body up on a bar until chin passes the bar",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Pull
            },
            new Skill
            {
                Id = Guid.Parse("f28ed423-4cb0-4451-90a5-2cf63723ad32"),
                Name = "Muscle-Up",
                Description = "Pull up and push over a bar in one smooth motion",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Pull
            },
            new Skill
            {
                Id = Guid.Parse("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"),
                Name = "Front Lever",
                Description = "Hold body horizontal while hanging from a bar",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Pull
            }
    };

    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        //builder
        //    .HasData(this._skills);
    }
}
