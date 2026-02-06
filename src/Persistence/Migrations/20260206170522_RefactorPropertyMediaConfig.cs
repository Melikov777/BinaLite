using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPropertyMediaConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyMedia_PropertyAd_PropertyAdId",
                table: "PropertyMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyMedia",
                table: "PropertyMedia");

            migrationBuilder.RenameTable(
                name: "PropertyMedia",
                newName: "PropertyMedias");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyMedia_PropertyAdId",
                table: "PropertyMedias",
                newName: "IX_PropertyMedias_PropertyAdId");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "PropertyMedias",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyMedias",
                table: "PropertyMedias",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_PropertyAdId_Order",
                table: "PropertyMedias",
                columns: new[] { "PropertyAdId", "Order" });

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyMedias_PropertyAd_PropertyAdId",
                table: "PropertyMedias",
                column: "PropertyAdId",
                principalTable: "PropertyAd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyMedias_PropertyAd_PropertyAdId",
                table: "PropertyMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyMedias",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_PropertyAdId_Order",
                table: "PropertyMedias");

            migrationBuilder.RenameTable(
                name: "PropertyMedias",
                newName: "PropertyMedia");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyMedias_PropertyAdId",
                table: "PropertyMedia",
                newName: "IX_PropertyMedia_PropertyAdId");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "PropertyMedia",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyMedia",
                table: "PropertyMedia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyMedia_PropertyAd_PropertyAdId",
                table: "PropertyMedia",
                column: "PropertyAdId",
                principalTable: "PropertyAd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
