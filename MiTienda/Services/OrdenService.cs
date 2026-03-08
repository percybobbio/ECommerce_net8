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
    }
}
