using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFoods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_foods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    serving_description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    calories = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    protein = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    carbs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    fat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    is_favorite = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_foods", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_foods_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_foods_user_id",
                table: "tbl_foods",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_foods_user_id_name",
                table: "tbl_foods",
                columns: new[] { "user_id", "name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_foods");
        }
    }
}
