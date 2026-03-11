namespace MiTienda.Models
{
    public class VentaItemVM
    {
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string PrecioUnitario { get; set; }
        public string ImagenProducto { get; set; }

        //Propiedades adicionales
        public decimal Total { get; set; }
}
}
