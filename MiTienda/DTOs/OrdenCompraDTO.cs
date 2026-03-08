using MiTienda.CapaEntidades;

namespace MiTienda.DTOs
{
    public class OrdenCompraDTO
    {
        public Venta Venta { get; set; }
        public Direccion Direccion { get; set; }
    }
}
