using System.ComponentModel.DataAnnotations;

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
        public string Contacto { get; set; }
        public string IdDistrito { get; set; }
        public Distrito? oDistrito { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string? IdTransaccion { get; set; }
        public DateTime FechaVenta { get; set; }
        public ICollection<DetalleVenta> oDetalleVenta { get; set; }
    }
}
