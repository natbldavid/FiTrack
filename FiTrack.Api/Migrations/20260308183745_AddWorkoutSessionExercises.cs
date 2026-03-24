using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutSessionExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_workout_session_exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workout_session_id = table.Column<int>(type: "int", nullable: false),
                    exercise_id = table.Column<int>(type: "int", nullable: false),
                    exercise_name_snapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    body_part_snapshot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    exercise_type_snapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    exercise_order = table.Column<int>(type: "int", nullable: false),
                    target_sets = table.Column<int>(type: "int", nullable: true),
                    target_reps_min = table.Column<int>(type: "int", nullable: true),
                    target_reps_max = table.Column<int>(type: "int", nullable: true),
                    planned_working_weight = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    actual_working_weight = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_session_exercises", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_session_exercises_exercise_order", "[exercise_order] > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_max", "[target_reps_max] IS NULL OR [target_reps_max] > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_min", "[target_reps_min] IS NULL OR [target_reps_min] > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_range", "([target_reps_min] IS NULL AND [target_reps_max] IS NULL) OR ([target_reps_min] IS NOT NULL AND [target_reps_max] IS NOT NULL AND [target_reps_max] >= [target_reps_min])");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_sets", "[target_sets] IS NULL OR [target_sets] > 0");
                    table.ForeignKey(
                        name: "FK_tbl_workout_session_exercises_lkp_exercise_catalog_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "lkp_exercise_catalog",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_workout_session_exercises_tbl_workout_sessions_workout_session_id",
                        column: x => x.workout_session_id,
                        principalTable: "tbl_workout_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_exercise_id",
                table: "tbl_workout_session_exercises",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_workout_session_id",
                table: "tbl_workout_session_exercises",
                column: "workout_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_workout_session_id_exercise_order",
                table: "tbl_workout_session_exercises",
                columns: new[] { "workout_session_id", "exercise_order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_workout_session_exercises");
        }
    }
}
