using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class MarcaService(GenericoRepository<Marca> _marcaRepository)
    {
        public async Task<IEnumerable<MarcaVM>> GetAllAsync()
        {
            var marcas = await _marcaRepository.GetAllAsync();
            var marcasVM = marcas.Select(item => new MarcaVM
            {
                IdMarca = item.IdMarca,
                Descripcion = item.Descripcion,
                Activo = item.Activo,

                //Propiedades adicionales para la vista, si es necesario
                EstadoColor = item.Activo ? "bg-success" : "bg-danger",
                EstadoTexto = item.Activo ? "Activo" : "Inactivo"
            }).ToList();

            return marcasVM;
        }

        public async Task AddAsync(MarcaVM viewModel)
        {
            var entidad = new Marca
            {
                Descripcion = viewModel.Descripcion,
                Activo = viewModel.Activo
            };

            await _marcaRepository.AddAsync(entidad);
        }

        public async Task<MarcaVM?> GetByIdAsync(int id)
        {
            var marca = await _marcaRepository.GetByIdAsync(id);
            var marcaVM = new MarcaVM();

            if(marca != null)
            {
                marcaVM.IdMarca = marca.IdMarca;
                marcaVM.Descripcion = marca.Descripcion;
                marcaVM.Activo = marca.Activo;
            }

            return marcaVM;
        }
        public async Task EditAsync(MarcaVM viewModel)
        {
            var entidad = await _marcaRepository.GetByIdAsync(viewModel.IdMarca);
            if (entidad != null)
            {
                entidad.Descripcion = viewModel.Descripcion;
                entidad.Activo = viewModel.Activo;
                
                await _marcaRepository.EditAsync(entidad);
            }
        }
        public async Task DeleteAsync(int id)
        {
            var marca = await _marcaRepository.GetByIdAsync(id);
            if(marca != null)
            {
                await _marcaRepository.DeleteAsync(marca);
            }
        }
    }
}
