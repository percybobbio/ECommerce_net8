namespace MiTienda.Models
{
    public class VentaVM
    {
        public int IdVenta { get; set; }
        public string FechaVenta { get; set; }
        public string MontoTotal { get; set; }
        public ICollection<VentaItemVM> Detalles { get; set; }
        public DireccionVM Direccion { get; set; }
    }
}
