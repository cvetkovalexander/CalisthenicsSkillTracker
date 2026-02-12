using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class MappingTableBetweenSkillsAndExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_SkillId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "ExerciseSkills",
                columns: table => new
                {
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSkills", x => new { x.ExerciseId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ExerciseSkills_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSkills_SkillId",
                table: "ExerciseSkills",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseSkills");

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_SkillId",
                table: "Exercises",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Skills_SkillId",
                table: "Exercises",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
