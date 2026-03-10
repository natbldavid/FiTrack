using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_food_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    logged_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    source_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: true),
                    meal_id = table.Column<int>(type: "int", nullable: true),
                    name_snapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    calories = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    carbs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    fat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 1m),
                    meal_slot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_food_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_food_logs_meal_slot", "[meal_slot] IS NULL OR [meal_slot] IN ('breakfast', 'lunch', 'dinner', 'snack')");
                    table.CheckConstraint("CK_tbl_food_logs_source_type", "[source_type] IN ('food', 'meal', 'custom')");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "tbl_foods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_meals_meal_id",
                        column: x => x.meal_id,
                        principalTable: "tbl_meals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_food_id",
                table: "tbl_food_logs",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_meal_id",
                table: "tbl_food_logs",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_user_id_log_date",
                table: "tbl_food_logs",
                columns: new[] { "user_id", "log_date" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_user_id_logged_at",
                table: "tbl_food_logs",
                columns: new[] { "user_id", "logged_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_food_logs");
        }
    }
}
