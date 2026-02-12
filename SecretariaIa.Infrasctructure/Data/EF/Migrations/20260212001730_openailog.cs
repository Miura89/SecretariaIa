using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretariaIa.Infrasctructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class openailog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenAiUsageLog",
                table: "OpenAiUsageLog");

            migrationBuilder.RenameTable(
                name: "OpenAiUsageLog",
                newName: "OpenAiUsageLogs");

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "OpenAiUsageLogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "DurationMs",
                table: "OpenAiUsageLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "OpenAiUsageLogs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityUserId",
                table: "OpenAiUsageLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InputCharacters",
                table: "OpenAiUsageLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InputType",
                table: "OpenAiUsageLogs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "OpenAiUsageLogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OutputCharacters",
                table: "OpenAiUsageLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "OpenAiUsageLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PromptVersion",
                table: "OpenAiUsageLogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "OpenAiUsageLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "OpenAiUsageLogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenAiUsageLogs",
                table: "OpenAiUsageLogs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_IdentityUserId",
                table: "OpenAiUsageLogs",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_PlanId",
                table: "OpenAiUsageLogs",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenAiUsageLogs_SubscriptionId",
                table: "OpenAiUsageLogs",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenAiUsageLogs_IdentityUser_IdentityUserId",
                table: "OpenAiUsageLogs",
                column: "IdentityUserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenAiUsageLogs_Plan_PlanId",
                table: "OpenAiUsageLogs",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenAiUsageLogs_Subscriptions_SubscriptionId",
                table: "OpenAiUsageLogs",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenAiUsageLogs_IdentityUser_IdentityUserId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenAiUsageLogs_Plan_PlanId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenAiUsageLogs_Subscriptions_SubscriptionId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenAiUsageLogs",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_IdentityUserId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_PlanId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropIndex(
                name: "IX_OpenAiUsageLogs_SubscriptionId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "DurationMs",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "InputCharacters",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "InputType",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "OutputCharacters",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "PromptVersion",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "OpenAiUsageLogs");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "OpenAiUsageLogs");

            migrationBuilder.RenameTable(
                name: "OpenAiUsageLogs",
                newName: "OpenAiUsageLog");

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "OpenAiUsageLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenAiUsageLog",
                table: "OpenAiUsageLog",
                column: "RequestId");
        }
    }
}
