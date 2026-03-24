using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseDemoGifToExerciseCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "exercise_demo_gif",
                table: "lkp_exercise_catalog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exercise_demo_gif",
                table: "lkp_exercise_catalog");
        }
    }
}
