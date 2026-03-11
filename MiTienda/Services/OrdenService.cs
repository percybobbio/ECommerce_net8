using MiTienda.CapaEntidades;
using MiTienda.DTOs;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class OrdenService(OrdenRepository _ordenRepository, DireccionService _direccionService)
    {
        public async Task AddAsync(List<CarritoItemVM> carritoItemVMs, int idCliente, DireccionVM direccionVM)
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
        }

        public async Task<List<VentaVM>> ObtenerOrdenesPorClienteAsync(int idCliente)
        {
            var ventas = await _ordenRepository.ObtenerVentasConDetalleAsync(idCliente);
            var ventaVMs = ventas.Select(v => new VentaVM
            {
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
    }

    
}
