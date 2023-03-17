using Microsoft.EntityFrameworkCore.Migrations;

namespace Curso.Core.Data.Migrations
{
    public partial class AddAjusteVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lenght",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lenght",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "Videos");
        }
    }
}
