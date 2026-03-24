using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lkp_activity_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lkp_activity_types", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lkp_activity_types_name",
                table: "lkp_activity_types",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lkp_activity_types");
        }
    }
}
