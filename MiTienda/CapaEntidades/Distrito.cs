using System.ComponentModel.DataAnnotations;

namespace MiTienda.CapaEntidades
{
    public class Distrito
    {
        public string IdDistrito { get; set; }
        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        public string Descripcion { get; set; }
        public string IdProvincia { get; set; }
        public Provincia? oProvincia { get; set; }
        public string IdDepartamento { get; set; }
        public Departamento? oDepartamento { get; set; }
    }
}
