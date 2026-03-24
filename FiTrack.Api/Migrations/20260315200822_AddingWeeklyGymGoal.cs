using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingWeeklyGymGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "weekly_gym_goal",
                table: "tbl_user_profile",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weekly_gym_goal",
                table: "tbl_user_profile");
        }
    }
}
