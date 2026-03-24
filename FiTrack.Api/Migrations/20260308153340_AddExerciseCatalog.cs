using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lkp_exercise_catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    body_part = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    exercise_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lkp_exercise_catalog", x => x.id);
                    table.CheckConstraint("CK_lkp_exercise_catalog_body_part", "[body_part] IS NULL OR [body_part] IN ('chest', 'shoulders', 'bicep', 'tricep', 'back', 'legs', 'core')");
                    table.CheckConstraint("CK_lkp_exercise_catalog_exercise_type", "[exercise_type] IN ('compound', 'accessory')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog",
                column: "body_part");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog",
                column: "exercise_type");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_is_active",
                table: "lkp_exercise_catalog",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_name",
                table: "lkp_exercise_catalog",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lkp_exercise_catalog");
        }
    }
}
