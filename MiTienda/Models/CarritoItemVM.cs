namespace MiTienda.Models
{
    public class CarritoItemVM
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string? NombreImagen { get; set; }
    }
}
