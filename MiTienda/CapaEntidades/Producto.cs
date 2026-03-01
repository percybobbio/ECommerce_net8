using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Required(ErrorMessage ="El Nombre del Producto es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripción del Producto es obligatoria")]
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "El Precio del Producto es obligatorio")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El Stock del Producto es obligatorio")]
        public int Stock { get; set; }
        public string? RutaImagen { get; set; }
        public string? NombreImagen { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdCategoria { get; set; }
        public Categoria oCategoria { get; set; }
        public int IdMarca { get; set; }
        public Marca oMarca { get; set; }
    }
}
