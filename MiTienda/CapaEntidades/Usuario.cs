using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
