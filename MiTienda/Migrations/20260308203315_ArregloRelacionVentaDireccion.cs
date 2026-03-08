using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiTienda.Migrations
{
    /// <inheritdoc />
    public partial class ArregloRelacionVentaDireccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_direccion_idVenta",
                table: "direccion");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idVenta",
                table: "direccion",
                column: "idVenta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_direccion_idVenta",
                table: "direccion");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idVenta",
                table: "direccion",
                column: "idVenta");
        }
    }
}
