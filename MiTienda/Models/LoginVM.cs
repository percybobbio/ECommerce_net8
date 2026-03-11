using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
    }
}
