using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Departamento
    {
        public string IdDepartamento { get; set; }
        [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
        public string Descripcion { get; set; }
    }
}
