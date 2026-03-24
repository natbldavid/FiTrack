using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMealItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_meal_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meal_id = table.Column<int>(type: "int", nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 1m),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_meal_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_meal_items_tbl_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "tbl_foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_meal_items_tbl_meals_meal_id",
                        column: x => x.meal_id,
                        principalTable: "tbl_meals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_food_id",
                table: "tbl_meal_items",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_meal_id",
                table: "tbl_meal_items",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_meal_id_food_id",
                table: "tbl_meal_items",
                columns: new[] { "meal_id", "food_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_meal_items");
        }
    }
}
