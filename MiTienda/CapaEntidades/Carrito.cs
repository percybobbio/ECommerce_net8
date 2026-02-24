namespace MiTienda.CapaEntidades
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
