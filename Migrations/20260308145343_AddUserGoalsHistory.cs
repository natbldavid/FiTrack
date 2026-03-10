using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGoalsHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_user_goals_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    effective_from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    effective_to = table.Column<DateTime>(type: "datetime2", nullable: true),
                    daily_calorie_goal = table.Column<int>(type: "int", nullable: true),
                    daily_protein_goal = table.Column<int>(type: "int", nullable: true),
                    daily_carb_goal = table.Column<int>(type: "int", nullable: true),
                    daily_fat_goal = table.Column<int>(type: "int", nullable: true),
                    weight_goal = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    weekly_exercise_goal = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_goals_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_goals_history_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goals_history_user_id",
                table: "tbl_user_goals_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goals_history_user_id_effective_from",
                table: "tbl_user_goals_history",
                columns: new[] { "user_id", "effective_from" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_user_goals_history");
        }
    }
}
