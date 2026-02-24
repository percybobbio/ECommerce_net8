using Microsoft.EntityFrameworkCore;
using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;

namespace MiTienda.Services
{
    public class CategoriaService(GenericoRepository<Categoria> _categoriaRepository)
    {
        public async Task<IEnumerable<CategoriaVM>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            var categoriasVM = categorias.Select(item => new CategoriaVM
            {
                IdCategoria = item.IdCategoria,
                Descripcion = item.Descripcion,
                Activo = item.Activo,

                //Propiedades adicionales para la vista
                EstadoTexto = item.Activo ? "Activo" : "Inactivo",
                EstadoColor = item.Activo ? "bg-success" : "bg-danger"
            }).ToList();

            return categoriasVM;
        }

        public async Task AddAsync(CategoriaVM viewModel)
        {
            var entidad = new Categoria
            {
                Descripcion = viewModel.Descripcion,
                Activo = viewModel.Activo
            };

            await _categoriaRepository.AddAsync(entidad);
        }

        public async Task<CategoriaVM?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            var categoriaVM = new CategoriaVM();

            if (categoria != null)
            {
                categoriaVM.IdCategoria = categoria.IdCategoria;
                categoriaVM.Descripcion = categoria.Descripcion;
                categoriaVM.Activo = categoria.Activo;
            }
            return categoriaVM;
        }

        public async Task EditAsync(CategoriaVM viewModel)
        {
            var entidad = await _categoriaRepository.GetByIdAsync(viewModel.IdCategoria);
            if (entidad != null)
            {
                entidad.Descripcion = viewModel.Descripcion;
                entidad.Activo = viewModel.Activo;
                
                await _categoriaRepository.EditAsync(entidad);
            };

        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if(categoria != null)
            {
                await _categoriaRepository.DeleteAsync(categoria);

            }
        }

    }
}
