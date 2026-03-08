using MiTienda.CapaEntidades;

namespace MiTienda.Models
{
    public class DireccionVM
    {
        public int IdDireccion { get; set; }
        public int IdVenta { get; set; }
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }
        public string DireccionCompleta { get; set; }
        public string IdDepartamento { get; set; }
        public string IdProvincia { get; set; }
        public string IdDistrito { get; set; }
    }
}
