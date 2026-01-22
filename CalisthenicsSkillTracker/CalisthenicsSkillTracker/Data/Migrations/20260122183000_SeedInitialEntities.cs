using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "MeasurementType", "Name", "SkillType" },
                values: new object[,]
                {
                    { new Guid("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"), 1, "A static hold balancing on hands with body parallel to ground", 3, "Planche", 1 },
                    { new Guid("9aa10e18-49f5-4b90-8613-74eec06259c0"), 1, "Pull body up on a bar until chin passes the bar", 2, "Pull-Up", 2 },
                    { new Guid("dcf2edd8-602f-4b96-8521-57b34c5ece9f"), 2, "Balance on hands, can be against a wall or freestanding", 3, "Handstand", 1 },
                    { new Guid("f28ed423-4cb0-4451-90a5-2cf63723ad32"), 1, "Pull up and push over a bar in one smooth motion", 2, "Muscle-Up", 2 },
                    { new Guid("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"), 1, "Hold body horizontal while hanging from a bar", 3, "Front Lever", 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9204), "elena.dimitrova@example.com", "Elena", "Dimitrova", "Calis2026!", "calisthenics_pro" },
                    { new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9202), "maria.ivanova@example.com", "Maria", "Ivanova", "Fit2026!", "fitgirl92" },
                    { new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9203), "daniel.georgiev@example.com", "Daniel", "Georgiev", "GytTime1!", "gymking" },
                    { new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9199), "alex.petrov@example.com", "Alex", "Petrov", "Pass123!", "athlete01" },
                    { new Guid("e70607c0-c065-43ce-b602-15b689784194"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9206), "ivan.kostov@example.com", "Ivan", "Kostov", "StrongMuscle1!", "muscle_man" }
                });

            migrationBuilder.InsertData(
                table: "SkillProgressRecords",
                columns: new[] { "Id", "Date", "Duration", "Notes", "Progression", "Repetitions", "SkillId", "UserId" },
                values: new object[,]
                {
                    { new Guid("0f28d0e8-06a8-4303-b355-7e3100c12981"), new DateTime(2026, 1, 19, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(8363), null, "Felt strong today", null, 10, new Guid("9aa10e18-49f5-4b90-8613-74eec06259c0"), new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e") },
                    { new Guid("1811c055-dd54-482d-ad03-7f160059749f"), new DateTime(2026, 1, 21, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(8371), 20, null, null, null, new Guid("dcf2edd8-602f-4b96-8521-57b34c5ece9f"), new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c") },
                    { new Guid("29eedd24-2050-4d7c-88d9-58982580bbfb"), new DateTime(2026, 1, 17, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(8351), 30, "Good form, need more wrist strength", 1, null, new Guid("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"), new Guid("e70607c0-c065-43ce-b602-15b689784194") },
                    { new Guid("743342d2-1d00-4168-b135-753d62d07127"), new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(8373), 10, "Core tight, need to extend legs fully", null, null, new Guid("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"), new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7") },
                    { new Guid("c9d51133-fa50-4904-b0cf-18fa9efe8429"), new DateTime(2026, 1, 20, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(8367), null, null, null, 3, new Guid("f28ed423-4cb0-4451-90a5-2cf63723ad32"), new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("0f28d0e8-06a8-4303-b355-7e3100c12981"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("1811c055-dd54-482d-ad03-7f160059749f"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("29eedd24-2050-4d7c-88d9-58982580bbfb"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("743342d2-1d00-4168-b135-753d62d07127"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("c9d51133-fa50-4904-b0cf-18fa9efe8429"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("9aa10e18-49f5-4b90-8613-74eec06259c0"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("dcf2edd8-602f-4b96-8521-57b34c5ece9f"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("f28ed423-4cb0-4451-90a5-2cf63723ad32"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e70607c0-c065-43ce-b602-15b689784194"));
        }
    }
}
