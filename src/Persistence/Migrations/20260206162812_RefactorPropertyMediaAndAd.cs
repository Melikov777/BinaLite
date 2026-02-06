using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPropertyMediaAndAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PropertyMedia");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "PropertyMedia");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "PropertyMedia",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "PropertyMedia");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PropertyMedia",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "PropertyMedia",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
