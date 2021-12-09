using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoDeLabWeb.Migrations
{
    public partial class addPreferemRestaurante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreferemRestaurante",
                columns: table => new
                {
                    IdPreferemRestaurante = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadorId = table.Column<int>(nullable: false),
                    RestauranteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferemRestaurante", x => x.IdPreferemRestaurante);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreferemRestaurante");
        }
    }
}
