using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class DetalleVenta
    {
        [Key]
        public int IdDetalleVenta { get; set; }

        // 1. La Llave Foránea (El ID)
        public int IdVenta { get; set; }

        // 2. La Propiedad de Navegación (El Objeto)
        public Venta? oVenta { get; set; }

        // 1. La Llave Foránea (El ID)
        public int IdProducto { get; set; }

        // 2. La Propiedad de Navegación (El Objeto)
        public Producto? oProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }
    }
}
