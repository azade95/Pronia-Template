using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class AddSurnameColumnInClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Clients");
        }
    }
}
