using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_workout_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    workout_day_id = table.Column<int>(type: "int", nullable: true),
                    session_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    session_date = table.Column<DateOnly>(type: "date", nullable: false),
                    started_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_sessions", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_sessions_status", "[status] IN ('in_progress', 'completed', 'cancelled')");
                    table.ForeignKey(
                        name: "FK_tbl_workout_sessions_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_workout_sessions_tbl_workout_days_workout_day_id",
                        column: x => x.workout_day_id,
                        principalTable: "tbl_workout_days",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_status",
                table: "tbl_workout_sessions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id",
                table: "tbl_workout_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_session_date",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "session_date" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_started_at",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_workout_day_id",
                table: "tbl_workout_sessions",
                column: "workout_day_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_workout_sessions");
        }
    }
}
