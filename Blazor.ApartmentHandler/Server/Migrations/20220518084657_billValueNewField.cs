using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.ApartmentHandler.Server.Migrations
{
    public partial class billValueNewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BillValue",
                table: "Bills",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillValue",
                table: "Bills");
        }
    }
}
