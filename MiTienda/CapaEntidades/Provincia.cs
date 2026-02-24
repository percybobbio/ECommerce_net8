using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Provincia
    {
        public string IdProvincia { get; set; }
        [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
        public string Descripcion { get; set; }
        public string IdDepartamento { get; set; }
        public Departamento oDepartamento { get; set; }
    }
}
