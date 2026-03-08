using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiTienda.Migrations
{
    /// <inheritdoc />
    public partial class CreacionDireccionYLimpiezaVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalleVenta_producto_IdProducto",
                table: "detalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_detalleVenta_venta_IdVenta",
                table: "detalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_cliente_ClienteIdCliente",
                table: "venta");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_cliente_IdCliente",
                table: "venta");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_distrito_idDistrito",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_venta_ClienteIdCliente",
                table: "venta");

            migrationBuilder.DropIndex(
                name: "IX_venta_idDistrito",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "ClienteIdCliente",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "contacto",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "direccion",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "idDistrito",
                table: "venta");

            migrationBuilder.DropColumn(
                name: "telefono",
                table: "venta");

            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "venta",
                newName: "idCliente");

            migrationBuilder.RenameIndex(
                name: "IX_venta_IdCliente",
                table: "venta",
                newName: "IX_venta_idCliente");

            migrationBuilder.RenameColumn(
                name: "IdVenta",
                table: "detalleVenta",
                newName: "idVenta");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "detalleVenta",
                newName: "idProducto");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVenta_IdVenta",
                table: "detalleVenta",
                newName: "IX_detalleVenta_idVenta");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVenta_IdProducto",
                table: "detalleVenta",
                newName: "IX_detalleVenta_idProducto");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "direccion",
                columns: table => new
                {
                    idDireccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idVenta = table.Column<int>(type: "int", nullable: false),
                    contacto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    direccionCompleta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    idDepartamento = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    idProvincia = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    idDistrito = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_direccion", x => x.idDireccion);
                    table.ForeignKey(
                        name: "FK_direccion_departamento_idDepartamento",
                        column: x => x.idDepartamento,
                        principalTable: "departamento",
                        principalColumn: "idDepartamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_direccion_distrito_idDistrito",
                        column: x => x.idDistrito,
                        principalTable: "distrito",
                        principalColumn: "idDistrito",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_direccion_provincia_idProvincia",
                        column: x => x.idProvincia,
                        principalTable: "provincia",
                        principalColumn: "idProvincia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_direccion_venta_idVenta",
                        column: x => x.idVenta,
                        principalTable: "venta",
                        principalColumn: "idVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idDepartamento",
                table: "direccion",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idDistrito",
                table: "direccion",
                column: "idDistrito");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idProvincia",
                table: "direccion",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_idVenta",
                table: "direccion",
                column: "idVenta");

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVenta_producto_idProducto",
                table: "detalleVenta",
                column: "idProducto",
                principalTable: "producto",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVenta_venta_idVenta",
                table: "detalleVenta",
                column: "idVenta",
                principalTable: "venta",
                principalColumn: "idVenta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_venta_cliente_idCliente",
                table: "venta",
                column: "idCliente",
                principalTable: "cliente",
                principalColumn: "idCliente",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalleVenta_producto_idProducto",
                table: "detalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_detalleVenta_venta_idVenta",
                table: "detalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_venta_cliente_idCliente",
                table: "venta");

            migrationBuilder.DropTable(
                name: "direccion");

            migrationBuilder.RenameColumn(
                name: "idCliente",
                table: "venta",
                newName: "IdCliente");

            migrationBuilder.RenameIndex(
                name: "IX_venta_idCliente",
                table: "venta",
                newName: "IX_venta_IdCliente");

            migrationBuilder.RenameColumn(
                name: "idVenta",
                table: "detalleVenta",
                newName: "IdVenta");

            migrationBuilder.RenameColumn(
                name: "idProducto",
                table: "detalleVenta",
                newName: "IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVenta_idVenta",
                table: "detalleVenta",
                newName: "IX_detalleVenta_IdVenta");

            migrationBuilder.RenameIndex(
                name: "IX_detalleVenta_idProducto",
                table: "detalleVenta",
                newName: "IX_detalleVenta_IdProducto");

            migrationBuilder.AddColumn<int>(
                name: "ClienteIdCliente",
                table: "venta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contacto",
                table: "venta",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "venta",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "idDistrito",
                table: "venta",
                type: "nchar(6)",
                fixedLength: true,
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "telefono",
                table: "venta",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "producto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_venta_ClienteIdCliente",
                table: "venta",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_venta_idDistrito",
                table: "venta",
                column: "idDistrito");

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVenta_producto_IdProducto",
                table: "detalleVenta",
                column: "IdProducto",
                principalTable: "producto",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_detalleVenta_venta_IdVenta",
                table: "detalleVenta",
                column: "IdVenta",
                principalTable: "venta",
                principalColumn: "idVenta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_venta_cliente_ClienteIdCliente",
                table: "venta",
                column: "ClienteIdCliente",
                principalTable: "cliente",
                principalColumn: "idCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_venta_cliente_IdCliente",
                table: "venta",
                column: "IdCliente",
                principalTable: "cliente",
                principalColumn: "idCliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_venta_distrito_idDistrito",
                table: "venta",
                column: "idDistrito",
                principalTable: "distrito",
                principalColumn: "idDistrito",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
