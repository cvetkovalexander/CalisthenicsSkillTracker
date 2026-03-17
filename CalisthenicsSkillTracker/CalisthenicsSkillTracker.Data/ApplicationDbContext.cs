using CalisthenicsSkillTracker.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            this.CalculateWorkoutDuration();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken token = default) 
        {
            CalculateWorkoutDuration();
            return await base.SaveChangesAsync(token);
        }

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

        private void CalculateWorkoutDuration() 
        {
            IEnumerable<Workout> workouts = ChangeTracker.Entries<Workout>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e.Entity);

            if (!workouts.Any())
                return;

            foreach (var workout in workouts)
            {
                // Handle cases where Finish is before Start (e.g., overnight workouts)
                if (workout.End >= workout.Start)
                {
                    workout.Duration = workout.End - workout.Start;
                }
                else
                {
                    workout.Duration = (TimeSpan.FromHours(24) - workout.Start) + workout.End;
                }
            }
        }
    }
}
