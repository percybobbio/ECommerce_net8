using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class DireccionService(GenericoRepository<Direccion> _direccionRepository)
    {
        public Direccion MapearEntidad(DireccionVM viewModel)
        {
            return new Direccion
            {
                Contacto = viewModel.Contacto,
                Telefono = viewModel.Telefono,
                DireccionCompleta = viewModel.DireccionCompleta,
                IdDepartamento = viewModel.IdDepartamento,
                IdProvincia = viewModel.IdProvincia,
                IdDistrito = viewModel.IdDistrito
            };
        }

        public async Task AddAsync(DireccionVM viewModel)
        {

        }
    }
}
