using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repertoar.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bioskop",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bioskop", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Datumi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumPrikazivanja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datumi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Filmovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmovi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PoceciPrikazivanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sati = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoceciPrikazivanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PoceciPrikazivanja_Datumi_DatumID",
                        column: x => x.DatumID,
                        principalTable: "Datumi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BioskopiFilmovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BioskopID = table.Column<int>(type: "int", nullable: true),
                    FilmID = table.Column<int>(type: "int", nullable: true),
                    PocetakPrikazivanjaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioskopiFilmovi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BioskopiFilmovi_Bioskop_BioskopID",
                        column: x => x.BioskopID,
                        principalTable: "Bioskop",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BioskopiFilmovi_Filmovi_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filmovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BioskopiFilmovi_PoceciPrikazivanja_PocetakPrikazivanjaID",
                        column: x => x.PocetakPrikazivanjaID,
                        principalTable: "PoceciPrikazivanja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BioskopiFilmovi_BioskopID",
                table: "BioskopiFilmovi",
                column: "BioskopID");

            migrationBuilder.CreateIndex(
                name: "IX_BioskopiFilmovi_FilmID",
                table: "BioskopiFilmovi",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_BioskopiFilmovi_PocetakPrikazivanjaID",
                table: "BioskopiFilmovi",
                column: "PocetakPrikazivanjaID");

            migrationBuilder.CreateIndex(
                name: "IX_PoceciPrikazivanja_DatumID",
                table: "PoceciPrikazivanja",
                column: "DatumID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BioskopiFilmovi");

            migrationBuilder.DropTable(
                name: "Bioskop");

            migrationBuilder.DropTable(
                name: "Filmovi");

            migrationBuilder.DropTable(
                name: "PoceciPrikazivanja");

            migrationBuilder.DropTable(
                name: "Datumi");
        }
    }
}
