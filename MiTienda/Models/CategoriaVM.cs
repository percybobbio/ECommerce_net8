using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class CategoriaVM
    {
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado de la categoría es obligatorio.")]
        public bool Activo { get; set; }

        //Propiedades adicionales
        public string? EstadoTexto { get; set; }
        public string? EstadoColor { get; set; }
    }
}
