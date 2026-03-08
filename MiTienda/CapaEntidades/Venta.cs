using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTienda.CapaEntidades
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public Cliente oCliente { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string? IdTransaccion { get; set; }
        public DateTime FechaVenta { get; set; }
        public ICollection<DetalleVenta> oDetalleVenta { get; set; }
        public virtual Direccion oDireccion { get; set; }
    }
}
