namespace MiTienda.Models
{
    public class VentaVM
    {
        public string FechaVenta { get; set; }
        public string MontoTotal { get; set; }
        public ICollection<VentaItemVM> Detalles { get; set; }
        public DireccionVM Direccion { get; set; }
    }
}
