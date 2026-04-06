using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Seeding;

public class ExerciseSeeding : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasData(this.GenerateExercises());
    }

    private List<Exercise> GenerateExercises()
    {
        return new List<Exercise>
        {
            new Exercise
            {
                Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                Name = "Incline Push Up",
                Description = "A beginner-friendly pushing exercise that builds pressing strength.",
                ImageUrl = "https://images.unsplash.com/photo-1609858922128-d24e042be1f1?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                Difficulty = Difficulty.Beginner,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Push,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                Name = "Standard Push Up",
                Description = "A fundamental bodyweight pushing exercise for chest, shoulders, and triceps.",
                ImageUrl = "https://plus.unsplash.com/premium_photo-1666956838404-02f23fa07123?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                Difficulty = Difficulty.Beginner,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Push,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                Name = "Pike Push Up",
                Description = "A vertical pushing exercise that prepares the body for handstand work.",
                ImageUrl = "https://images.unsplash.com/photo-1599744331048-d58b430fb098?w=400&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8ZGVjbGluZSUyMHB1c2glMjB1cHxlbnwwfHwwfHx8MA%3D%3D",
                Difficulty = Difficulty.Intermediate,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Push,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c4444444-4444-4444-4444-444444444444"),
                Name = "Bodyweight Row",
                Description = "A horizontal pulling exercise that develops upper back strength.",
                ImageUrl = "https://www.kettlebellkings.com/cdn/shop/articles/Inverted-Row.jpg?v=1703750562",
                Difficulty = Difficulty.Beginner,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Pull,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c5555555-5555-5555-5555-555555555555"),
                Name = "Pull Up",
                Description = "A classic vertical pulling exercise that builds serious upper body strength.",
                ImageUrl = "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?auto=format&fit=crop&w=1200&q=80",
                Difficulty = Difficulty.Intermediate,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Pull,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c6666666-6666-6666-6666-666666666666"),
                Name = "Dip",
                Description = "A compound pushing exercise for chest, shoulders, and triceps.",
                ImageUrl = "https://training.fit/wp-content/uploads/2020/02/dips.png",
                Difficulty = Difficulty.Intermediate,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Push,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c7777777-7777-7777-7777-777777777777"),
                Name = "Hanging Knee Raise",
                Description = "A core exercise that improves abdominal strength and compression.",
                ImageUrl = "https://images.unsplash.com/photo-1517963879433-6ad2b056d712?auto=format&fit=crop&w=1200&q=80",
                Difficulty = Difficulty.Beginner,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.Core,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c8888888-8888-8888-8888-888888888888"),
                Name = "L-Sit Hold",
                Description = "A static core hold that builds compression and shoulder stability.",
                ImageUrl = "https://gravity.fitness/cdn/shop/articles/male-athlete-doing-l-sit-exercise-on-bricks-2021-08-27-19-27-31-utc.jpg?v=1651655207",
                Difficulty = Difficulty.Intermediate,
                MeasurementType = Measurement.Duration,
                ExerciseType = SkillType.Core,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("c9999999-9999-9999-9999-999999999999"),
                Name = "Handstand Hold",
                Description = "A static inverted hold focused on balance and shoulder endurance.",
                ImageUrl = "https://cdn.yogajournal.com/wp-content/uploads/2021/11/Handstand_Andrew-Clark.jpg",
                Difficulty = Difficulty.Advanced,
                MeasurementType = Measurement.Duration,
                ExerciseType = SkillType.Push,
                Category = Category.Balance
            },
            new Exercise
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Name = "Pistol Squat",
                Description = "A unilateral lower body exercise requiring balance, mobility, and strength.",
                ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/pistol-squat-regular-pistol-squat-233-1654102650.jpg?crop=0.560xw:0.839xh;0.209xw,0.161xh&resize=640:*",
                Difficulty = Difficulty.Advanced,
                MeasurementType = Measurement.Repetitions,
                ExerciseType = SkillType.LowerBody,
                Category = Category.Strength
            },
            new Exercise
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Name = "Front Lever Tuck Hold",
                Description = "A scaled front lever exercise emphasizing body tension and pulling strength.",
                ImageUrl = "https://workout-temple.com/wp-content/uploads/2022/11/sideview-tucked-front-lever.jpg",
                Difficulty = Difficulty.Advanced,
                MeasurementType = Measurement.Duration,
                ExerciseType = SkillType.Pull,
                Category = Category.Strength
            }
        };
    }
}
