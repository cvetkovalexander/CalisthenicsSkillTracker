using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configuration;

public class SkillProgressEntityTypeConfiguration : IEntityTypeConfiguration<SkillProgress>
{
    private readonly SkillProgress[] _records =
    {
        //new SkillProgress
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("e70607c0-c065-43ce-b602-15b689784194"), 
        //        SkillId = Guid.Parse("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"), 
        //        Date = DateTime.UtcNow.AddDays(-5),
        //        Duration = 30,
        //        Progression = CalisthenicsSkillTracker.Data.Models.Enums.Progression.Tuck,
        //        Notes = "Good form, need more wrist strength"
        //    },

        //    new SkillProgress
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("776ada73-4ef6-4c12-8934-1e79dbb5e03e"), 
        //        SkillId = Guid.Parse("9aa10e18-49f5-4b90-8613-74eec06259c0"), 
        //        Date = DateTime.UtcNow.AddDays(-3),
        //        Repetitions = 10,
        //        Notes = "Felt strong today"
        //    },

        //    new SkillProgress
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("91fca50a-7cdd-430f-b08c-dbdaa597ba26"), 
        //        SkillId = Guid.Parse("f28ed423-4cb0-4451-90a5-2cf63723ad32"),
        //        Date = DateTime.UtcNow.AddDays(-2),
        //        Repetitions = 3
        //    },

        //    new SkillProgress
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("82042202-ba9c-44f5-86fb-4b191dbf966c"), 
        //        SkillId = Guid.Parse("dcf2edd8-602f-4b96-8521-57b34c5ece9f"), 
        //        Date = DateTime.UtcNow.AddDays(-1),
        //        Duration = 20
        //    },

        //    new SkillProgress
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("c7cebde1-1c04-4bff-a388-d42961e5ece7"), 
        //        SkillId = Guid.Parse("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"), 
        //        Date = DateTime.UtcNow,
        //        Duration = 10,
        //        Notes = "Core tight, need to extend legs fully"
        //    }
    };

    public void Configure(EntityTypeBuilder<SkillProgress> builder)
    {
        //builder
        //    .HasData(this._records);
    }
}
