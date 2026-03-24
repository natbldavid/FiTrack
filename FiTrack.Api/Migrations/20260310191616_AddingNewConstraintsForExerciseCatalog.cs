using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewConstraintsForExerciseCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog");

            migrationBuilder.DropCheckConstraint(
                name: "CK_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog");

            migrationBuilder.AddCheckConstraint(
                name: "CK_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog",
                sql: "[body_part] IS NULL OR [body_part] IN ('Chest', 'Shoulders', 'Bicep', 'Tricep', 'Back', 'Quadriceps', 'Hamstring', 'Calf', 'Abs')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog",
                sql: "[exercise_type] IN ('Compound', 'Accessory', 'Core', 'Cardio')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog");

            migrationBuilder.DropCheckConstraint(
                name: "CK_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog");

            migrationBuilder.AddCheckConstraint(
                name: "CK_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog",
                sql: "[body_part] IS NULL OR [body_part] IN ('chest', 'shoulders', 'bicep', 'tricep', 'back', 'legs', 'core')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog",
                sql: "[exercise_type] IN ('compound', 'accessory')");
        }
    }
}
