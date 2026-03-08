using Microsoft.AspNetCore.Http;
using MiTienda.Models;
using MiTienda.Utilities;

namespace MiTienda.Services
{
    public class CarritoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public CarritoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public void AgregarAlCarrito(ProductoVM producto, int cantidad)
        {
            var carrito = _session.GetCarrito<List<CarritoItemVM>>("Carrito") ?? new List<CarritoItemVM>();
            var itemExistente = carrito.Find(i => i.IdProducto == producto.IdProducto);

            if(itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItemVM
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = cantidad,
                    NombreImagen = producto.NombreImagen
                });
            }

            _session.SetCarrito("Carrito", carrito);
        }

        // Método que necesitaremos pronto para mostrar el total o los items
        public List<CarritoItemVM> ObtenerCarrito()
        {
            return _session.GetCarrito<List<CarritoItemVM>>("Carrito") ?? new List<CarritoItemVM>();
        }

        public void EliminarDelCarrito(int IdProducto)
        {
            var carrito = _session.GetCarrito<List<CarritoItemVM>>("Carrito") ?? new List<CarritoItemVM>();
            var itemEliminar = carrito.Find(i => i.IdProducto == IdProducto);
            if (itemEliminar != null)
            {
                carrito.Remove(itemEliminar!);
                _session.SetCarrito("Carrito", carrito);
            }
        }

        public void VaciarCarrito()
        {
            _session.Remove("Carrito");
        }
    }
}
