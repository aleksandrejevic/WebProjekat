using Microsoft.EntityFrameworkCore.Migrations;

namespace Repertoar.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Godina",
                table: "Filmovi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OriginalniNaziv",
                table: "Filmovi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reziser",
                table: "Filmovi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Trajanje",
                table: "Filmovi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Uloge",
                table: "Filmovi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zanr",
                table: "Filmovi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZemljaPorekla",
                table: "Filmovi",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Godina",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "OriginalniNaziv",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "Reziser",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "Trajanje",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "Uloge",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "Zanr",
                table: "Filmovi");

            migrationBuilder.DropColumn(
                name: "ZemljaPorekla",
                table: "Filmovi");
        }
    }
}
