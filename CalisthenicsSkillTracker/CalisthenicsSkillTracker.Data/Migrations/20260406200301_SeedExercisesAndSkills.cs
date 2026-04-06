using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedExercisesAndSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "Description", "Difficulty", "ExerciseType", "ImageUrl", "MeasurementType", "Name" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), 1, "A beginner-friendly pushing exercise that builds pressing strength.", 1, 1, "https://images.unsplash.com/photo-1609858922128-d24e042be1f1?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", 1, "Incline Push Up" },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), 1, "A fundamental bodyweight pushing exercise for chest, shoulders, and triceps.", 1, 1, "https://plus.unsplash.com/premium_photo-1666956838404-02f23fa07123?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", 1, "Standard Push Up" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), 1, "A vertical pushing exercise that prepares the body for handstand work.", 2, 1, "https://images.unsplash.com/photo-1599744331048-d58b430fb098?w=400&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8ZGVjbGluZSUyMHB1c2glMjB1cHxlbnwwfHwwfHx8MA%3D%3D", 1, "Pike Push Up" },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), 1, "A horizontal pulling exercise that develops upper back strength.", 1, 2, "https://www.kettlebellkings.com/cdn/shop/articles/Inverted-Row.jpg?v=1703750562", 1, "Bodyweight Row" },
                    { new Guid("c5555555-5555-5555-5555-555555555555"), 1, "A classic vertical pulling exercise that builds serious upper body strength.", 2, 2, "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?auto=format&fit=crop&w=1200&q=80", 1, "Pull Up" },
                    { new Guid("c6666666-6666-6666-6666-666666666666"), 1, "A compound pushing exercise for chest, shoulders, and triceps.", 2, 1, "https://training.fit/wp-content/uploads/2020/02/dips.png", 1, "Dip" },
                    { new Guid("c7777777-7777-7777-7777-777777777777"), 1, "A core exercise that improves abdominal strength and compression.", 1, 3, "https://images.unsplash.com/photo-1517963879433-6ad2b056d712?auto=format&fit=crop&w=1200&q=80", 1, "Hanging Knee Raise" },
                    { new Guid("c8888888-8888-8888-8888-888888888888"), 1, "A static core hold that builds compression and shoulder stability.", 2, 3, "https://gravity.fitness/cdn/shop/articles/male-athlete-doing-l-sit-exercise-on-bricks-2021-08-27-19-27-31-utc.jpg?v=1651655207", 2, "L-Sit Hold" },
                    { new Guid("c9999999-9999-9999-9999-999999999999"), 2, "A static inverted hold focused on balance and shoulder endurance.", 3, 1, "https://cdn.yogajournal.com/wp-content/uploads/2021/11/Handstand_Andrew-Clark.jpg", 2, "Handstand Hold" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 1, "A unilateral lower body exercise requiring balance, mobility, and strength.", 3, 4, "https://hips.hearstapps.com/hmg-prod/images/pistol-squat-regular-pistol-squat-233-1654102650.jpg?crop=0.560xw:0.839xh;0.209xw,0.161xh&resize=640:*", 1, "Pistol Squat" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 1, "A scaled front lever exercise emphasizing body tension and pulling strength.", 3, 2, "https://workout-temple.com/wp-content/uploads/2022/11/sideview-tucked-front-lever.jpg", 2, "Front Lever Tuck Hold" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "Difficulty", "ImageUrl", "MeasurementType", "Name", "SkillType" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1, "A foundational pushing skill that builds upper body strength and control.", 1, "https://www.sixstarpro.com/cdn/shop/articles/how-to-get-better-at-push-ups_c96cd61d-63d8-4bd0-a93e-e3a8dee52408.jpg?v=1726000547", 1, "Push Up", 1 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 1, "A classic pulling skill that develops back, arm, and grip strength.", 2, "https://velocitywestchester.com/wp-content/uploads/2025/06/rsw_1280-9.webp", 1, "Pull Up", 2 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 1, "A compound pushing skill focused on chest, shoulders, and triceps.", 2, "https://training.fit/wp-content/uploads/2020/02/dips.png", 1, "Dip", 1 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 1, "A static core skill requiring hip flexor and abdominal strength.", 2, "https://gravity.fitness/cdn/shop/articles/male-athlete-doing-l-sit-exercise-on-bricks-2021-08-27-19-27-31-utc.jpg?v=1651655207", 2, "L-Sit", 3 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 2, "An inverted balance skill that develops body awareness and shoulder stability.", 3, "https://cdn.yogajournal.com/wp-content/uploads/2021/11/Handstand_Andrew-Clark.jpg", 2, "Handstand", 1 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 1, "A demanding static pulling skill requiring full-body tension.", 4, "https://caliathletics.com/wp-content/uploads/2018/09/Frontback-lever.jpg", 2, "Front Lever", 2 },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 1, "A static hold that challenges shoulder mobility and pulling strength.", 4, "https://cdn.shopify.com/s/files/1/0568/6280/2107/files/back_lever_1a0f7520-1b68-426f-91a2-7c192a81df24_480x480.jpg", 2, "Back Lever", 2 },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 1, "An elite pushing skill performed with the body held parallel to the ground.", 4, "https://andrystrong.com/wp-content/uploads/2023/10/bas-1024x576.webp", 2, "Planche", 1 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 1, "A single-leg lower body skill demanding strength, balance, and mobility.", 3, "https://hips.hearstapps.com/hmg-prod/images/pistol-squat-regular-pistol-squat-233-1654102650.jpg?crop=0.560xw:0.839xh;0.209xw,0.161xh&resize=640:*", 1, "Pistol Squat", 4 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 1, "A powerful core skill emphasizing anti-extension strength and control.", 3, "https://cdn.shopify.com/s/files/1/0568/6280/2107/files/dragon_flag_600x600.jpg", 1, "Dragon Flag", 3 },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 5, "An advanced static skill combining lateral core strength and pulling power.", 4, "https://www.streetworkoutstkilda.com/wp-content/uploads/2020/08/human-flag-beast-1.jpeg", 2, "Human Flag", 3 }
                });

            migrationBuilder.InsertData(
                table: "ExerciseSkills",
                columns: new[] { "ExerciseId", "SkillId" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), new Guid("55555555-5555-5555-5555-555555555555") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExerciseSkills",
                keyColumns: new[] { "ExerciseId", "SkillId" },
                keyValues: new object[] { new Guid("c1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "ExerciseSkills",
                keyColumns: new[] { "ExerciseId", "SkillId" },
                keyValues: new object[] { new Guid("c2222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "ExerciseSkills",
                keyColumns: new[] { "ExerciseId", "SkillId" },
                keyValues: new object[] { new Guid("c3333333-3333-3333-3333-333333333333"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));
        }
    }
}
