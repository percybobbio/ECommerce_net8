using MiTienda.CapaEntidades;
using System.ComponentModel.DataAnnotations;

namespace MiTienda.Models
{
    public class DireccionVM
    {
        public int IdDireccion { get; set; }
        public int IdVenta { get; set; }
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string DireccionCompleta { get; set; }
        [Required(ErrorMessage = "El departamento es obligatorio.")]
        public string IdDepartamento { get; set; }
        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string IdProvincia { get; set; }
        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public string IdDistrito { get; set; }

        //Propiedades de navegación para mostrar los nombres en la vista
        public string NombreDepartamento { get; set; }
        public string NombreProvincia { get; set; }
        public string NombreDistrito { get; set; }
    }
}
