using CalisthenicsSkillTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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

        public virtual DbSet<Workout> Workouts { get; set; } = null!;

        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; } = null!;

        public virtual DbSet<Exercise> Exercises { get; set; } = null!;

        public virtual DbSet<WorkoutSet> WorkoutSets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Exercise>()
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

            builder
                .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
