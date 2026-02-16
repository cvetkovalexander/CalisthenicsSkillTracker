using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalisthenicsSkillTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimeSpanFieldsForWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Workouts",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "Workouts",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "Workouts",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Workouts");
        }
    }
}
