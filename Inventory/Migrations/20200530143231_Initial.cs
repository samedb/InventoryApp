using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Predmeti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(nullable: false),
                    Marka = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Cena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmeti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Radnici",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    JMBG = table.Column<string>(nullable: false),
                    Ime = table.Column<string>(nullable: false),
                    Prezime = table.Column<string>(nullable: false),
                    Pol = table.Column<string>(nullable: false),
                    StrucnaSprema = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Radnici", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Prostorije",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NazivProstorije = table.Column<string>(nullable: false),
                    Sprat = table.Column<string>(nullable: true),
                    Sirina = table.Column<string>(nullable: true),
                    Visina = table.Column<string>(nullable: true),
                    Duzina = table.Column<string>(nullable: true),
                    SefProstorijeUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prostorije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prostorije_Radnici_SefProstorijeUsername",
                        column: x => x.SefProstorijeUsername,
                        principalTable: "Radnici",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PredmetId = table.Column<int>(nullable: false),
                    ProstorijaId = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventar_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventar_Prostorije_ProstorijaId",
                        column: x => x.ProstorijaId,
                        principalTable: "Prostorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zaduzenja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RadnikUsername = table.Column<string>(nullable: false),
                    PredmetId = table.Column<int>(nullable: false),
                    ProstorijaId = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    DatumZaduzivanja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaduzenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaduzenja_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zaduzenja_Prostorije_ProstorijaId",
                        column: x => x.ProstorijaId,
                        principalTable: "Prostorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zaduzenja_Radnici_RadnikUsername",
                        column: x => x.RadnikUsername,
                        principalTable: "Radnici",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventar_PredmetId",
                table: "Inventar",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventar_ProstorijaId",
                table: "Inventar",
                column: "ProstorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prostorije_SefProstorijeUsername",
                table: "Prostorije",
                column: "SefProstorijeUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Zaduzenja_PredmetId",
                table: "Zaduzenja",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaduzenja_ProstorijaId",
                table: "Zaduzenja",
                column: "ProstorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaduzenja_RadnikUsername",
                table: "Zaduzenja",
                column: "RadnikUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventar");

            migrationBuilder.DropTable(
                name: "Zaduzenja");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "Prostorije");

            migrationBuilder.DropTable(
                name: "Radnici");
        }
    }
}
