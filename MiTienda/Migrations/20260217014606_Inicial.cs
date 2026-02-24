using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiTienda.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    clave = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    reestablecer = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.idCliente);
                });

            migrationBuilder.CreateTable(
                name: "departamento",
                columns: table => new
                {
                    idDepartamento = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departamento", x => x.idDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "marca",
                columns: table => new
                {
                    idMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marca", x => x.idMarca);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    clave = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    reestablecer = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "provincia",
                columns: table => new
                {
                    idProvincia = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    idDepartamento = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincia", x => x.idProvincia);
                    table.ForeignKey(
                        name: "FK_provincia_departamento_idDepartamento",
                        column: x => x.idDepartamento,
                        principalTable: "departamento",
                        principalColumn: "idDepartamento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    stock = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    rutaImagen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    nombreImagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    idMarca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.idProducto);
                    table.CheckConstraint("CK_Producto_Precio", "precio >= 0");
                    table.CheckConstraint("CK_Producto_Stock", "stock >= 0");
                    table.ForeignKey(
                        name: "FK_producto_categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "categoria",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_producto_marca_idMarca",
                        column: x => x.idMarca,
                        principalTable: "marca",
                        principalColumn: "idMarca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "distrito",
                columns: table => new
                {
                    idDistrito = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    idProvincia = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    idDepartamento = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distrito", x => x.idDistrito);
                    table.CheckConstraint("CK_Distrito_Departamento_Consistente", "idDepartamento = LEFT(idProvincia, 2)");
                    table.ForeignKey(
                        name: "FK_distrito_departamento_idDepartamento",
                        column: x => x.idDepartamento,
                        principalTable: "departamento",
                        principalColumn: "idDepartamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_distrito_provincia_idProvincia",
                        column: x => x.idProvincia,
                        principalTable: "provincia",
                        principalColumn: "idProvincia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    idCarrito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito", x => x.idCarrito);
                    table.CheckConstraint("CK_Carrito_Cantidad", "cantidad > 0");
                    table.ForeignKey(
                        name: "FK_carrito_cliente_idCliente",
                        column: x => x.idCliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_carrito_producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    idVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    totalProducto = table.Column<int>(type: "int", nullable: false),
                    montoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    contacto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idDistrito = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    idTransaccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "sysutcdatetime()"),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.idVenta);
                    table.ForeignKey(
                        name: "FK_venta_cliente_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente");
                    table.ForeignKey(
                        name: "FK_venta_cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_venta_distrito_idDistrito",
                        column: x => x.idDistrito,
                        principalTable: "distrito",
                        principalColumn: "idDistrito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalleVenta",
                columns: table => new
                {
                    idDetalleVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleVenta", x => x.idDetalleVenta);
                    table.CheckConstraint("CK_DetalleVenta_Cantidad", "cantidad > 0");
                    table.CheckConstraint("CK_DetalleVenta_Total", "total >= 0");
                    table.ForeignKey(
                        name: "FK_detalleVenta_producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_detalleVenta_venta_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "venta",
                        principalColumn: "idVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categoria",
                columns: new[] { "idCategoria", "descripcion" },
                values: new object[,]
                {
                    { 1, "Electrónica" },
                    { 2, "Dormitorio" },
                    { 3, "Tecnologia" }
                });

            migrationBuilder.InsertData(
                table: "marca",
                columns: new[] { "idMarca", "descripcion" },
                values: new object[,]
                {
                    { 1, "Samsung" },
                    { 2, "LG" },
                    { 3, "Sony" }
                });

            migrationBuilder.InsertData(
                table: "producto",
                columns: new[] { "idProducto", "descripcion", "idCategoria", "idMarca", "nombre", "nombreImagen", "precio", "rutaImagen", "stock" },
                values: new object[,]
                {
                    { 1, "Televisor inteligente de 55 pulgadas", 1, 1, "Smart TV Samsung 55\"", null, 799.99m, null, 10 },
                    { 2, "Refrigeradora de alta eficiencia", 1, 2, "Refrigeradora LG", null, 1199.99m, null, 5 },
                    { 3, "Laptop para uso diario", 3, 3, "Laptop Sony", null, 499.99m, null, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_carrito_idCliente",
                table: "carrito",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_carrito_idProducto",
                table: "carrito",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_categoria_descripcion",
                table: "categoria",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cliente_correo",
                table: "cliente",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalleVenta_IdProducto",
                table: "detalleVenta",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_detalleVenta_IdVenta",
                table: "detalleVenta",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_distrito_idDepartamento",
                table: "distrito",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_distrito_idProvincia",
                table: "distrito",
                column: "idProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_marca_descripcion",
                table: "marca",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_producto_idCategoria",
                table: "producto",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_producto_idMarca",
                table: "producto",
                column: "idMarca");

            migrationBuilder.CreateIndex(
                name: "IX_provincia_idDepartamento",
                table: "provincia",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_correo",
                table: "usuario",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_venta_ClienteIdCliente",
                table: "venta",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_venta_IdCliente",
                table: "venta",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_venta_idDistrito",
                table: "venta",
                column: "idDistrito");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrito");

            migrationBuilder.DropTable(
                name: "detalleVenta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "marca");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "distrito");

            migrationBuilder.DropTable(
                name: "provincia");

            migrationBuilder.DropTable(
                name: "departamento");
        }
    }
}
