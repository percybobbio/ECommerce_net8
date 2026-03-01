using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiTienda.CapaEntidades;
using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class ProductoVM
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El Nombre del Producto es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripción del Producto es obligatoria")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El Precio del Producto es obligatorio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El Stock del Producto es obligatorio")]
        public int Stock { get; set; }


        public string? RutaImagen { get; set; }
        public IFormFile? ArchivoImagen { get; set; } // Para recibir el archivo desde el formulario
        public string? NombreImagen { get; set; }

        public bool Activo { get; set; }

        //Propiedades de otras tablas
        [ValidateNever]
        public CategoriaVM Categoria { get; set; }

        [ValidateNever]
        // Para llenar el dropdown de categorías en la vista, necesitamos una lista de SelectListItem
        public List<SelectListItem> listaCategorias { get; set; }

        [ValidateNever]
        public MarcaVM Marca { get; set; }

        [ValidateNever]
        public List<SelectListItem> listaMarcas { get; set; }

        //Propiedades Adicionales
        public string? EstadoTexto { get; set; }

        public string? EstadoColor { get; set; }

        [ValidateNever]
        public DateTime? FechaRegistro { get; set; }

    }
}
