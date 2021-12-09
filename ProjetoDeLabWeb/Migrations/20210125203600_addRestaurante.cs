using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoDeLabWeb.Migrations
{
    public partial class addRestaurante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurante",
                columns: table => new
                {
                    IdRestaurante = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    Morada = table.Column<string>(nullable: true),
                    Gps = table.Column<string>(nullable: true),
                    HorarioFunc = table.Column<string>(nullable: true),
                    DiaDescanco = table.Column<string>(nullable: true),
                    TipoServico = table.Column<string>(nullable: true),
                    RestauranteAceite = table.Column<bool>(nullable: false),
                    HorarioCriado = table.Column<bool>(nullable: false),
                    SegundaFeira_PratoDoDia = table.Column<bool>(nullable: false),
                    TercaFeira_PratoDoDia = table.Column<bool>(nullable: false),
                    QuartaFeira_PratoDoDia = table.Column<bool>(nullable: false),
                    QuintaFeira_PratoDoDia = table.Column<bool>(nullable: false),
                    SextaFeira_PratoDoDia = table.Column<bool>(nullable: false),
                    Sabado_PratoDoDia = table.Column<bool>(nullable: false),
                    Domingo_PratoDoDia = table.Column<bool>(nullable: false),
                    NdePreferem = table.Column<int>(nullable: false),
                    UtilizadorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurante", x => x.IdRestaurante);
                    table.ForeignKey(
                        name: "FK_Restaurante_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurante_UtilizadorId",
                table: "Restaurante",
                column: "UtilizadorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restaurante");
        }
    }
}
