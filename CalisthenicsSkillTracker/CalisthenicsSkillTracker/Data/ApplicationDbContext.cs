using CalisthenicsSkillTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<SkillProgress> SkillProgressRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<SkillProgress>()
                .HasOne(sp => sp.PerformedBy)
                .WithMany(u => u.SkillProgressRecords)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SkillProgress>()
                .HasOne(sp => sp.Skill)
                .WithMany(s => s.SkillProgressRecords)
                .HasForeignKey(sp => sp.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
