using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado de la categoría es obligatorio.")]
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
