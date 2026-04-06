using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsSkillTracker.Data.Seeding;

public class SkillSeeding : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasData(this.GenerateSkills());
    }

    private List<Skill> GenerateSkills()
    {
        return new List<Skill>
        {
            new Skill
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Push Up",
                Description = "A foundational pushing skill that builds upper body strength and control.",
                ImageUrl = "https://www.sixstarpro.com/cdn/shop/articles/how-to-get-better-at-push-ups_c96cd61d-63d8-4bd0-a93e-e3a8dee52408.jpg?v=1726000547",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Beginner
            },
            new Skill
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Pull Up",
                Description = "A classic pulling skill that develops back, arm, and grip strength.",
                ImageUrl = "https://velocitywestchester.com/wp-content/uploads/2025/06/rsw_1280-9.webp",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Pull,
                Difficulty = Difficulty.Intermediate
            },
            new Skill
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Dip",
                Description = "A compound pushing skill focused on chest, shoulders, and triceps.",
                ImageUrl = "https://training.fit/wp-content/uploads/2020/02/dips.png",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Intermediate
            },
            new Skill
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "L-Sit",
                Description = "A static core skill requiring hip flexor and abdominal strength.",
                ImageUrl = "https://gravity.fitness/cdn/shop/articles/male-athlete-doing-l-sit-exercise-on-bricks-2021-08-27-19-27-31-utc.jpg?v=1651655207",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Core,
                Difficulty = Difficulty.Intermediate
            },
            new Skill
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Handstand",
                Description = "An inverted balance skill that develops body awareness and shoulder stability.",
                ImageUrl = "https://cdn.yogajournal.com/wp-content/uploads/2021/11/Handstand_Andrew-Clark.jpg",
                MeasurementType = Measurement.Duration,
                Category = Category.Balance,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Name = "Front Lever",
                Description = "A demanding static pulling skill requiring full-body tension.",
                ImageUrl = "https://caliathletics.com/wp-content/uploads/2018/09/Frontback-lever.jpg",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Pull,
                Difficulty = Difficulty.Expert
            },
            new Skill
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                Name = "Back Lever",
                Description = "A static hold that challenges shoulder mobility and pulling strength.",
                ImageUrl = "https://cdn.shopify.com/s/files/1/0568/6280/2107/files/back_lever_1a0f7520-1b68-426f-91a2-7c192a81df24_480x480.jpg",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Pull,
                Difficulty = Difficulty.Expert
            },
            new Skill
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Name = "Planche",
                Description = "An elite pushing skill performed with the body held parallel to the ground.",
                ImageUrl = "https://andrystrong.com/wp-content/uploads/2023/10/bas-1024x576.webp",
                MeasurementType = Measurement.Duration,
                Category = Category.Strength,
                SkillType = SkillType.Push,
                Difficulty = Difficulty.Expert
            },
            new Skill
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Name = "Pistol Squat",
                Description = "A single-leg lower body skill demanding strength, balance, and mobility.",
                ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/pistol-squat-regular-pistol-squat-233-1654102650.jpg?crop=0.560xw:0.839xh;0.209xw,0.161xh&resize=640:*",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.LowerBody,
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Dragon Flag",
                Description = "A powerful core skill emphasizing anti-extension strength and control.",
                ImageUrl = "https://cdn.shopify.com/s/files/1/0568/6280/2107/files/dragon_flag_600x600.jpg",
                MeasurementType = Measurement.Repetitions,
                Category = Category.Strength,
                SkillType = SkillType.Core,
                Difficulty = Difficulty.Advanced
            },
            new Skill
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Human Flag",
                Description = "An advanced static skill combining lateral core strength and pulling power.",
                ImageUrl = "https://www.streetworkoutstkilda.com/wp-content/uploads/2020/08/human-flag-beast-1.jpeg",
                MeasurementType = Measurement.Duration,
                Category = Category.Coordination,
                SkillType = SkillType.Core,
                Difficulty = Difficulty.Expert
            }
        };
    }
}
