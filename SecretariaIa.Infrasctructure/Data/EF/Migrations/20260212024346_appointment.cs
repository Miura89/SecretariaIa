using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretariaIa.Infrasctructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class appointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RemindBeforeMinutes = table.Column<int>(type: "integer", nullable: false),
                    ReminderSent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExcludedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ExcludedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_IdentityUser_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_IdentityUserId_Timestamp",
                table: "OpenAiUsageLogs",
                columns: new[] { "IdentityUserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_OperationType",
                table: "OpenAiUsageLogs",
                column: "OperationType");

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_Success",
                table: "OpenAiUsageLogs",
                column: "Success");

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_Timestamp",
                table: "OpenAiUsageLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_IdentityUserId",
                table: "Appointments",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_IdentityUserId_Timestamp",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_OperationType",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_Success",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_Timestamp",
                table: "OpenAiUsageLogs");
        }
    }
}
