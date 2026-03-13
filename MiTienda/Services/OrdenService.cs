using MiTienda.CapaEntidades;
using MiTienda.DTOs;
using MiTienda.Models;
using MiTienda.Repositories;
using SelectPdf;

namespace MiTienda.Services
{
    public class OrdenService(OrdenRepository _ordenRepository, DireccionService _direccionService)
    {
        public async Task<int> AddAsync(List<CarritoItemVM> carritoItemVMs, int idCliente, DireccionVM direccionVM)
        {
            var entidadDireccion = _direccionService.MapearEntidad(direccionVM);
            //1. Mapeamos los datos del carrito a la entidad Venta
            Venta venta = new Venta
            {
                IdCliente = idCliente,
                TotalProducto = carritoItemVMs.Sum(x => x.Cantidad),
                MontoTotal = carritoItemVMs.Sum(x => x.Precio * x.Cantidad),
                FechaVenta = DateTime.Now.ToUniversalTime(),
                IdTransaccion = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(), // Generamos un ID de transacción único, se cambiara por pasarela de pagos
                oDetalleVenta = carritoItemVMs.Select(dv => new DetalleVenta
                {
                    //IdVenta se asignará automáticamente al guardar la venta
                    IdProducto = dv.IdProducto,
                    Cantidad = dv.Cantidad,
                    Total = dv.Precio
                }).ToList(),

                //2. Mapeamos los datos de la dirección a la entidad Direccion
                oDireccion = entidadDireccion
            };

            //3. Guardamos la orden completa en la base de datos
            await _ordenRepository.AddAsync(venta);

            return venta.IdVenta; // Devolvemos el ID de la venta recién creada para futuras referencias (como generar el recibo)
        }

        public async Task<List<VentaVM>> ObtenerOrdenesPorClienteAsync(int idCliente)
        {
            var ventas = await _ordenRepository.ObtenerVentasConDetalleAsync(idCliente);
            var ventaVMs = ventas.Select(v => new VentaVM
            {
                IdVenta = v.IdVenta,
                FechaVenta = v.FechaVenta.ToString("dd/MM/yyyy"),
                MontoTotal = v.MontoTotal.ToString("C2"),
                Detalles = v.oDetalleVenta?.Select(dv => new VentaItemVM
                {
                    //Transformar el string de imagen para que sea una foto
                    ImagenProducto = "/images/" + (dv.oProducto?.NombreImagen ?? "sin-foto.jpg"),
                    NombreProducto = dv.oProducto?.Nombre ?? "Producto Desconocido",
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.Total.ToString("C2"),
                    Total = dv.Total * dv.Cantidad
                }).ToList() ?? new List<VentaItemVM>(),

                Direccion = v.oDireccion == null ? new DireccionVM() : new DireccionVM
                {
                    Contacto = v.oDireccion?.Contacto ?? "Sin Contacto",
                    Telefono = v.oDireccion?.Telefono ?? "Sin telefono",
                    DireccionCompleta = v.oDireccion?.DireccionCompleta ?? "No se colocó dirección",
                    // A traves de distrito navegamos a los demas
                    NombreDistrito = string.IsNullOrWhiteSpace(v.oDireccion?.oDistrito?.Descripcion)
                                     ? "N/A"
                                     : v.oDireccion.oDistrito.Descripcion,
                    //Accedemos a Provincia y Departamento a traves de Distrito que incluye a ambos
                    NombreProvincia = string.IsNullOrWhiteSpace(v.oDireccion?.oDistrito?.oProvincia?.Descripcion)
                                     ? "N/A"
                                     : v.oDireccion.oDistrito.oProvincia.Descripcion,
                    NombreDepartamento = string.IsNullOrWhiteSpace(v.oDireccion?.oDistrito?.oProvincia?.oDepartamento?.Descripcion)
                                        ? "N/A"
                                        : v.oDireccion.oDistrito.oProvincia.oDepartamento.Descripcion
                }
            }).ToList();

            return ventaVMs;
        }

        public async Task<byte[]> GenerarReciboPdfAsync(int idVenta, int idCliente)
        {
            //1. Aquí podrías obtener los detalles de la orden para incluirlos en el recibo
            var historialVentas = await ObtenerOrdenesPorClienteAsync(idCliente);

            var venta = historialVentas.FirstOrDefault(v => v.IdVenta == idVenta);

            if (venta == null)
            {
                throw new Exception("No se encontró la orden para generar el recibo.");
            }

            //2. Se diseña el recibo
            string htmlString = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: 'Arial', sans-serif; padding: 20px; color: #333; }}
                    .title-store {{ color: #0d6efd; font-size: 28px; margin: 0; }}
                    .subtitle {{ color: #6c757d; font-size: 14px; margin-top: 5px; }}
                    .divider {{ border-bottom: 2px solid #0d6efd; margin-bottom: 20px; }}
            
                    /* Tabla para alinear las dos columnas de datos perfectamente */
                    .layout-table {{ width: 100%; margin-bottom: 30px; }}
                    .layout-table td {{ vertical-align: top; width: 50%; }}
            
                    h4 {{ margin: 0 0 10px 0; color: #2c3e50; border-bottom: 1px solid #eee; padding-bottom: 5px; }}
                    p {{ margin: 0 0 5px 0; font-size: 13px; }}
            
                    /* Tabla de productos */
                    .product-table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
                    .product-table th {{ background-color: #f8f9fa; border: 1px solid #dee2e6; padding: 10px; text-align: left; font-size: 13px; }}
                    .product-table td {{ border: 1px solid #dee2e6; padding: 10px; font-size: 13px; }}
            
                    /* Alineaciones */
                    .text-right {{ text-align: right !important; }}
                    .text-center {{ text-align: center !important; }}
            
                    /* Caja del total */
                    .total-box {{ width: 200px; float: right; border: 2px solid #198754; padding: 15px; border-radius: 5px; background-color: #f8fff9; text-align: right; }}
                    .total-box h3 {{ margin: 0; color: #198754; font-size: 20px; }}
                </style>
            </head>
            <body>
                <div style='text-align: center;' class='divider'>
                    <h1 class='title-store'>MiTienda - E-Commerce</h1>
                    <p class='subtitle'>Recibo de Compra Oficial</p>
                </div>
        
                <table class='layout-table'>
                    <tr>
                        <td>
                            <h4>Detalles del Cliente</h4>
                            <p><strong>Pedido #:</strong> {venta.IdVenta}</p>
                            <p><strong>Fecha de Emisión:</strong> {venta.FechaVenta}</p>
                        </td>
                        <td class='text-right'>
                            <h4>Datos de Envío</h4>
                            <p><strong>Contacto:</strong> {venta.Direccion.Contacto}</p>
                            <p><strong>Teléfono:</strong> {venta.Direccion.Telefono}</p>
                            <p><strong>Dirección:</strong> {venta.Direccion.DireccionCompleta}</p>
                            <p>{venta.Direccion.NombreDistrito}, {venta.Direccion.NombreProvincia}, {venta.Direccion.NombreDepartamento}</p>
                        </td>
                    </tr>
                </table>
        
                <table class='product-table'>
                    <thead>
                        <tr>
                            <th>Producto</th>
                            <th class='text-center'>Precio Unitario</th>
                            <th class='text-center'>Cant.</th>
                            <th class='text-right'>Total</th>
                        </tr>
                    </thead>
                    <tbody>";

                    // BUCLE PARA LOS PRODUCTOS (Ojo: Quité la imagen para que quede limpio como factura real)
                    foreach (var item in venta.Detalles)
                    {
                        htmlString += $@"
                        <tr>
                            <td>{item.NombreProducto}</td>
                            <td class='text-center'>{item.PrecioUnitario}</td>
                            <td class='text-center'>{item.Cantidad}</td>
                            <td class='text-right'>{item.Total.ToString("C2")}</td>
                        </tr>";
                    }

                    htmlString += $@"
                    </tbody>
                </table>
        
                <div class='total-box'>
                    <h3>Total: {venta.MontoTotal}</h3>
                </div>
        
                <div style='clear: both;'></div>
        
                <div style='margin-top: 60px; text-align: center; color: #888; font-size: 11px; border-top: 1px dashed #ccc; padding-top: 10px;'>
                    Gracias por confiar en MiTienda. Este documento es un comprobante electrónico válido.
                </div>
            </body>
            </html>";

            // ==========================================
            // 5. CONVERTIR Y DESCARGAR
            // ==========================================

            //6. Instanciamos el convertidor de HTML a PDF
            HtmlToPdf conversor = new HtmlToPdf();

            //7. Configuramos opciones adicionales si es necesario (tamaño de página, márgenes, etc.)
            conversor.Options.PdfPageSize = PdfPageSize.A4;
            conversor.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            conversor.Options.WebPageWidth = 1024; // Ancho para que se vea bien el diseño
            conversor.Options.WebPageHeight = 0; // Altura automática para ajustarse al contenido
            conversor.Options.MarginTop = 30;
            conversor.Options.MarginBottom = 30;
            conversor.Options.MarginLeft = 30;
            conversor.Options.MarginRight = 30;

            //8. Convertimos el HTML a PDF
            PdfDocument documento = conversor.ConvertHtmlString(htmlString);

            //9. Lo convertimos a un arreglo de bytes para descargarlo
            byte[] pdfBytes = documento.Save();
            documento.Close(); // Cerramos el documento para liberar recursos

            //10. Devolvemos el PDF
            return pdfBytes;
        }
    }


}
