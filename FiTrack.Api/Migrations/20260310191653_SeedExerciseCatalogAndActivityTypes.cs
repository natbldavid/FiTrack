using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedExerciseCatalogAndActivityTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var timestamp = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "lkp_exercise_catalog",
                columns: new[] { "name", "body_part", "exercise_type", "is_active", "created_at", "updated_at" },
                values: new object[,]
                {
                    { "Romanian Deadlift", "Hamstring", "Compound", true, timestamp, timestamp },
                    { "Seated Row", "Back", "Compound", true, timestamp, timestamp },
                    { "Walking Lunges", "Quadriceps", "Compound", true, timestamp, timestamp },
                    { "Rear Deltoid Machine", "Back", "Accessory", true, timestamp, timestamp },
                    { "Hammer Curls", "Bicep", "Accessory", true, timestamp, timestamp },
                    { "Calf Raises", "Calf", "Accessory", true, timestamp, timestamp },
                    { "Hanging Leg Raises", "Abs", "Core", true, timestamp, timestamp },
                    { "Dumbbell Bench Press", "Chest", "Compound", true, timestamp, timestamp },
                    { "Leg Press", "Quadriceps", "Compound", true, timestamp, timestamp },
                    { "Dumbbell Shoulder Press", "Shoulders", "Compound", true, timestamp, timestamp },
                    { "Cable Chest Fly", "Chest", "Accessory", true, timestamp, timestamp },
                    { "Tricep Pushdown", "Tricep", "Accessory", true, timestamp, timestamp },
                    { "Lateral Raises", "Shoulders", "Accessory", true, timestamp, timestamp },
                    { "Sit-Ups", "Abs", "Core", true, timestamp, timestamp },
                    { "Lat Pulldown", "Back", "Compound", true, timestamp, timestamp },
                    { "Incline Dumbbell Press", "Chest", "Compound", true, timestamp, timestamp },
                    { "Squats", "Quadriceps", "Compound", true, timestamp, timestamp },
                    { "Barbell Curls", "Bicep", "Accessory", true, timestamp, timestamp },
                    { "Tricep Overhead Cable", "Tricep", "Accessory", true, timestamp, timestamp },
                    { "Shrugs", "Shoulders", "Accessory", true, timestamp, timestamp },
                    { "Plank", "Abs", "Core", true, timestamp, timestamp }
                });

            migrationBuilder.InsertData(
                table: "lkp_activity_types",
                columns: new[] { "name", "icon", "created_at" },
                values: new object[,]
                {
                    { "Football", "⚽", timestamp },
                    { "Gym", "🏋️", timestamp },
                    { "Running", "🏃", timestamp },
                    { "Squash", "🎾", timestamp },
                    { "Swimming", "🏊", timestamp },
                    { "Cricket", "🏏", timestamp },
                    { "Rugby", "🏉", timestamp }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM [lkp_exercise_catalog]
WHERE [name] IN (
    'Romanian Deadlift',
    'Seated Row',
    'Walking Lunges',
    'Rear Deltoid Machine',
    'Hammer Curcls',
    'Calf Raises',
    'Hanging Leg Raises',
    'Dumbbell Bench Press',
    'Leg Press',
    'Dumbbell Shoulder Press',
    'Cable Chest Fly',
    'Tricep Pushdown',
    'Lateral Raises',
    'Sit-Ups',
    'Lat Pulldown',
    'Incline Dumbbell Press',
    'Squats',
    'Barbell Curls',
    'Tricep Overhead Cable',
    'Shrugs',
    'Plank'
);");

            migrationBuilder.Sql(@"
DELETE FROM [lkp_activity_types]
WHERE [name] IN (
    'Football',
    'Gym',
    'Running',
    'Squash',
    'Swimming',
    'Cricket',
    'Rugby'
);");
        }
    }
}
