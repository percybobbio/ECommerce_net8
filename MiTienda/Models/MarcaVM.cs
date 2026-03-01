using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class MarcaVM
    {
        public int IdMarca { get; set; }
        [Required   (ErrorMessage = "El nombre de la marca es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado de la marca es obligatorio.")]
        public bool Activo { get; set; }

        //Propiedades adicionales
        public string? EstadoTexto { get; set; }
        public string? EstadoColor { get; set; }
    }
}
