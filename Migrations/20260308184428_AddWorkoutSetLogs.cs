using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutSetLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_workout_set_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workout_session_exercise_id = table.Column<int>(type: "int", nullable: false),
                    set_number = table.Column<int>(type: "int", nullable: false),
                    reps = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    completed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_set_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_set_logs_reps", "[reps] >= 0");
                    table.CheckConstraint("CK_tbl_workout_set_logs_set_number", "[set_number] > 0");
                    table.ForeignKey(
                        name: "FK_tbl_workout_set_logs_tbl_workout_session_exercises_workout_session_exercise_id",
                        column: x => x.workout_session_exercise_id,
                        principalTable: "tbl_workout_session_exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_set_logs_workout_session_exercise_id",
                table: "tbl_workout_set_logs",
                column: "workout_session_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_set_logs_workout_session_exercise_id_set_number",
                table: "tbl_workout_set_logs",
                columns: new[] { "workout_session_exercise_id", "set_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_workout_set_logs");
        }
    }
}
