using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedExerciseEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises");

            migrationBuilder.AlterColumn<Guid>(
                name: "SkillId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "Description", "Difficulty", "ExerciseType", "Name", "SkillId" },
                values: new object[,]
                {
                    { new Guid("678990c4-6079-4355-8557-52c9bf98e068"), 1, "Core-focused exercise improving L-sit strength and control", 1, 3, "L-Sit Raises", new Guid("6cc6e3eb-d9f6-47c9-6b9b-08de60d0ec0e") },
                    { new Guid("adb90954-64e2-465e-8cd2-8806de209ed2"), 1, "A vertical pushing exercise progressing towards a handstand push-up", 1, 1, "Pike Push-up", new Guid("433d8f6c-40df-4a94-633d-08de6290695f") },
                    { new Guid("b3460318-f3ba-437a-8ab8-99b06a8cb796"), 1, "Vertical pushing exercise targeting chest and triceps", 1, 1, "Dip", null },
                    { new Guid("ecf724ba-50cd-4bc3-82a1-e8623426afd1"), 1, "Pulling exercise progressing towards full front lever", 1, 2, "Tuck Front Lever Pull-up", new Guid("ff8ad0b0-6b1a-47d5-b2d5-a6df0e59b54c") }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises");

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("678990c4-6079-4355-8557-52c9bf98e068"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("adb90954-64e2-465e-8cd2-8806de209ed2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("b3460318-f3ba-437a-8ab8-99b06a8cb796"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("ecf724ba-50cd-4bc3-82a1-e8623426afd1"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SkillId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
