using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo Clave es obligatorio.")]
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }
        public DateTime FechaRegistro { get; set; }

        // RELACIÓN: Un cliente tiene muchas ventas
        // Usamos 'virtual' para permitir que EF cargue las ventas solo cuando las necesites (Lazy Loading)
        public ICollection<Venta>? oVentas { get; set; }
    }
}
