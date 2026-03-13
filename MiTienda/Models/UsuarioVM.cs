using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class UsuarioVM
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo Clave es obligatorio.")]
        public string Clave { get; set; }
        public bool Reestablecer { get; set; } = false;
        public bool Activo { get; set; } = true;

        //Propiedades adicionales
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}
