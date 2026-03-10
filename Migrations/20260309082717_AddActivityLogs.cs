using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_activity_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    activity_type_id = table.Column<int>(type: "int", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    distance = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    calories_burned = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_activity_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_activity_logs_duration", "[duration_minutes] > 0");
                    table.ForeignKey(
                        name: "FK_tbl_activity_logs_lkp_activity_types_activity_type_id",
                        column: x => x.activity_type_id,
                        principalTable: "lkp_activity_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_logs_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_activity_type_id",
                table: "tbl_activity_logs",
                column: "activity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_user_id",
                table: "tbl_activity_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_user_id_log_date",
                table: "tbl_activity_logs",
                columns: new[] { "user_id", "log_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_activity_logs");
        }
    }
}
