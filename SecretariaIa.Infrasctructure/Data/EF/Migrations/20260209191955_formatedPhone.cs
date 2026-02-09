using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretariaIa.Infrasctructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class formatedPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Confidence",
                table: "MessagesLog",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "IdentityUser",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "FormatedPhone",
                table: "IdentityUser",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "FormatedPhone",
                table: "IdentityUser");

            migrationBuilder.AlterColumn<decimal>(
                name: "Confidence",
                table: "MessagesLog",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
