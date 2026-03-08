using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MiTienda.CapaEntidades
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }
        [Required]
        public int IdVenta { get; set; }
        [ForeignKey("IdVenta")]
        public Venta oVenta { get; set; }
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }
        [Required]
        [MaxLength(200)]
        public string DireccionCompleta { get; set; }
        [Required]
        public string IdDepartamento { get; set; }
        [ForeignKey("IdDepartamento")]
        public Departamento oDepartamento { get; set; }
        [Required]
        public string IdProvincia { get; set; }
        [ForeignKey("IdProvincia")]
        public Provincia oProvincia { get; set; }
        [Required]
        public string IdDistrito { get; set; }
        [ForeignKey("IdDistrito")]
        public Distrito oDistrito { get; set; }
    }
}
