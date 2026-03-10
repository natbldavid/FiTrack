using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoUserandUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tbl_users",
                newName: "id");

            migrationBuilder.CreateTable(
                name: "tbl_user_profile",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    current_weight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    daily_calorie_goal = table.Column<int>(type: "int", nullable: true),
                    daily_protein_goal = table.Column<int>(type: "int", nullable: true),
                    daily_carb_goal = table.Column<int>(type: "int", nullable: true),
                    daily_fat_goal = table.Column<int>(type: "int", nullable: true),
                    weight_goal = table.Column<int>(type: "int", nullable: true),
                    weekly_exercise_goal = table.Column<int>(type: "int", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_profile", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_profile_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tbl_users",
                columns: new[] { "id", "created_at", "passcode_hash", "updated_at", "username" },
                values: new object[] { 1, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEMOCK8GkM8J7W0X0dM5mA3O3Nn7Q3qV2Pq1r0J9G4k8x6mVh2Qq8qP1p4L5j9A==", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "demo_user" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_user_profile");

            migrationBuilder.DeleteData(
                table: "tbl_users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_users",
                newName: "Id");
        }
    }
}
