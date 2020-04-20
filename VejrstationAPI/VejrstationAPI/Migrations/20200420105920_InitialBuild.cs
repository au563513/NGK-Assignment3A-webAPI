using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VejrstationAPI.Migrations
{
    public partial class InitialBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Steder",
                columns: table => new
                {
                    Navn = table.Column<string>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steder", x => x.Navn);
                });

            migrationBuilder.CreateTable(
                name: "Vejrobservationer",
                columns: table => new
                {
                    VejrobservationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tidspunkt = table.Column<DateTime>(nullable: false),
                    StedNavn = table.Column<string>(nullable: true),
                    Temperatur = table.Column<double>(nullable: false),
                    Luftfugtighed = table.Column<int>(nullable: false),
                    Lufttryk = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vejrobservationer", x => x.VejrobservationId);
                    table.ForeignKey(
                        name: "FK_Vejrobservationer_Steder_StedNavn",
                        column: x => x.StedNavn,
                        principalTable: "Steder",
                        principalColumn: "Navn",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vejrobservationer_StedNavn",
                table: "Vejrobservationer",
                column: "StedNavn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vejrobservationer");

            migrationBuilder.DropTable(
                name: "Steder");
        }
    }
}
