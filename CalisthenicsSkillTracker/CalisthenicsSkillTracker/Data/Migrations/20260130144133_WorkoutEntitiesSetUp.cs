using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class WorkoutEntitiesSetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseType = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Progression = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_WorkoutExercises_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "WorkoutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SkillProgressRecords",
                columns: new[] { "Id", "Date", "Duration", "Notes", "Progression", "Repetitions", "SkillId", "UserId" },
                values: new object[,]
                {
                    { new Guid("5d214d26-8638-42d3-9114-47c045e1c6f5"), new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(2766), 10, "Core tight, need to extend legs fully", null, null, new Guid("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c"), new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7") },
                    { new Guid("84868d62-89a0-4891-a295-7220f4c7753b"), new DateTime(2026, 1, 25, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(2748), 30, "Good form, need more wrist strength", 1, null, new Guid("82bb355d-d50d-472c-8ac4-0f6dc3c86cb2"), new Guid("e70607c0-c065-43ce-b602-15b689784194") },
                    { new Guid("a3752308-47ec-4a15-b4f4-4b95f307335f"), new DateTime(2026, 1, 28, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(2762), null, null, null, 3, new Guid("f28ed423-4cb0-4451-90a5-2cf63723ad32"), new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26") },
                    { new Guid("f08aff51-1ba7-49fc-a8eb-f661bc61a9a5"), new DateTime(2026, 1, 27, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(2758), null, "Felt strong today", null, 10, new Guid("9aa10e18-49f5-4b90-8613-74eec06259c0"), new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e") },
                    { new Guid("f8e09ccd-adc5-4e7f-ae38-e7625abd40f7"), new DateTime(2026, 1, 29, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(2764), 20, null, null, null, new Guid("dcf2edd8-602f-4b96-8521-57b34c5ece9f"), new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c") }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(3641));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(3638));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e70607c0-c065-43ce-b602-15b689784194"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 30, 14, 41, 33, 429, DateTimeKind.Utc).AddTicks(3644));

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_SkillId",
                table: "Exercises",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutExerciseId",
                table: "WorkoutSets",
                column: "WorkoutExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("5d214d26-8638-42d3-9114-47c045e1c6f5"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("84868d62-89a0-4891-a295-7220f4c7753b"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("a3752308-47ec-4a15-b4f4-4b95f307335f"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("f08aff51-1ba7-49fc-a8eb-f661bc61a9a5"));

            migrationBuilder.DeleteData(
                table: "SkillProgressRecords",
                keyColumn: "Id",
                keyValue: new Guid("f8e09ccd-adc5-4e7f-ae38-e7625abd40f7"));

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("776ada73-4ef6-4c12-8934-1e79dbb5e03e"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9204));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("82042202-ba9c-44f5-86fb-4b191dbf966c"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9202));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("91fca50a-7cdd-430f-b08c-dbdaa597ba26"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c7cebde1-1c04-4bff-a388-d42961e5ece7"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9199));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e70607c0-c065-43ce-b602-15b689784194"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 22, 18, 30, 0, 39, DateTimeKind.Utc).AddTicks(9206));
        }
    }
}
