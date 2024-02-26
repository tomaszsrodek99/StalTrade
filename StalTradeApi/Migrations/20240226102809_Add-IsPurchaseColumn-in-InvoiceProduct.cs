using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StalTradeAPI.Migrations
{
    public partial class AddIsPurchaseColumninInvoiceProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPurchase",
                table: "InvoiceProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchase",
                table: "InvoiceProducts");
        }
    }
}
