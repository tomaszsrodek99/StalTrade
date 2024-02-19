using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StalTradeAPI.Migrations
{
    public partial class Updateprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Prices",
                newName: "Netto");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Prices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchase",
                table: "Prices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "IsPurchase",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "Netto",
                table: "Prices",
                newName: "Value");

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    PriceHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPurchase = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => x.PriceHistoryID);
                    table.ForeignKey(
                        name: "FK_PriceHistory_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_ProductID",
                table: "PriceHistory",
                column: "ProductID");
        }
    }
}
