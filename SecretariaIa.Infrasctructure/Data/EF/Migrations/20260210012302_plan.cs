using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretariaIa.Infrasctructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class plan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "IdentityUser",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "text", nullable: false),
                    PlanDescription = table.Column<string>(type: "text", nullable: false),
                    PriceUSD = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxMessagesPerMonth = table.Column<int>(type: "integer", nullable: false),
                    MaxOpenAiUsdPerMonth = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LimitBehaviore = table.Column<int>(type: "integer", nullable: false),
                    DefaultMode = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExcludedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ExcludedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_PlanId",
                table: "IdentityUser",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUser_Plan_PlanId",
                table: "IdentityUser",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUser_Plan_PlanId",
                table: "IdentityUser");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUser_PlanId",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "IdentityUser");
        }
    }
}
