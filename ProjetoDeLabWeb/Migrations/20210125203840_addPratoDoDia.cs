using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoDeLabWeb.Migrations
{
    public partial class addPratoDoDia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PratoDoDia",
                columns: table => new
                {
                    IdPratoDoDia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    DiaDaSemana = table.Column<string>(nullable: true),
                    DataPrato = table.Column<DateTime>(nullable: false),
                    Tipo = table.Column<string>(nullable: true),
                    Folga = table.Column<bool>(nullable: false),
                    NdePreferemPratoDoDia = table.Column<int>(nullable: false),
                    RestauranteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PratoDoDia", x => x.IdPratoDoDia);
                    table.ForeignKey(
                        name: "FK_PratoDoDia_Restaurante_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurante",
                        principalColumn: "IdRestaurante",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PratoDoDia_RestauranteId",
                table: "PratoDoDia",
                column: "RestauranteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PratoDoDia");
        }
    }
}
