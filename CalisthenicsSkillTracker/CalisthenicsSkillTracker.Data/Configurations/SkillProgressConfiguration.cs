using CalisthenicsSkillTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Configurations;

public class SkillProgressConfiguration : IEntityTypeConfiguration<SkillProgress>
{
    public void Configure(EntityTypeBuilder<SkillProgress> builder)
    {
        builder
            .HasOne(sp => sp.PerformedBy)
            .WithMany(u => u.SkillProgressRecords)
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(sp => sp.Skill)
            .WithMany(s => s.SkillProgressRecords)
            .HasForeignKey(sp => sp.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
