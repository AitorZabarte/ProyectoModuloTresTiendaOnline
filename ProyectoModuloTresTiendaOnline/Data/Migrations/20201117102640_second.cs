using Microsoft.EntityFrameworkCore.Migrations;

namespace ProyectoModuloTresTiendaOnline.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioFinal",
                table: "ProductoPedido",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PedidoFinalizado",
                table: "Pedido",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioFinal",
                table: "ProductoPedido");

            migrationBuilder.DropColumn(
                name: "PedidoFinalizado",
                table: "Pedido");
        }
    }
}
