using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }
        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado de la marca es obligatorio.")]
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
