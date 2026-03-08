using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;
using System.Linq.Expressions;

namespace MiTienda.Services
{
    public class ProductoService(
        GenericoRepository<Producto> _productoRepository,
        GenericoRepository<Categoria> _categoriaRepository,
        GenericoRepository<Marca> _marcaRepository,
        IWebHostEnvironment _webHostEnvironment
        )
    {
        public async Task<IEnumerable<ProductoVM>> GetAllAsync()
        {
            // 1. Obtener todos los productos con sus categorías y marcas relacionadas con includes y conditionals nullables
            var includes = new Expression<Func<Producto, object>>[]
            {
                p => p.oCategoria!, // Incluir la categoría
                p => p.oMarca! // Incluir la marca
            };

            //2. Llamamos al repositorio pasando 'null' a las condiciones y el array de includes para traer las entidades relacionadas
            var productos = await _productoRepository.GetAllAsync(
                    conditions: null, // No aplicamos condiciones, queremos todos los productos
                    includes: includes // Pasamos el array de includes para traer categorías y marcas
                );

            var productoVMs = productos.Select(item => new ProductoVM
            {
                IdProducto = item.IdProducto,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                RutaImagen = item.RutaImagen,
                NombreImagen = item.NombreImagen,
                Activo = item.Activo,
                Categoria = new CategoriaVM
                {
                    IdCategoria = item.oCategoria!.IdCategoria,
                    Descripcion = item.oCategoria!.Descripcion
                },
                Marca = new MarcaVM
                {
                    IdMarca = item.oMarca!.IdMarca,
                    Descripcion = item.oMarca!.Descripcion
                },

                //Propiedades adicionales para mostrar el estado en la vista
                EstadoTexto = item.Activo ? "Activo" : "Inactivo",
                EstadoColor = item.Activo ? "bg-success" : "bg-danger",
                FechaRegistro = item.FechaRegistro
            }).ToList();

            return productoVMs;
        }

        public async Task<ProductoVM> GetByIdAsync(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            var categoria = await _categoriaRepository.GetAllAsync();
            var marca = await _marcaRepository.GetAllAsync();

            // 🌟 FILTRO INTELIGENTE: Trae las activas, O la que el producto ya tiene asignada
            var categoriasFiltradas = categoria.Where(c =>
                c.Activo == true || (producto != null && c.IdCategoria == producto!.IdCategoria));

            var marcasFiltradas = marca.Where(m =>
                m.Activo == true || (producto != null && m.IdMarca == producto.IdMarca));

            var productoVM = new ProductoVM();

            //Si el producto existe, se asignan sus propiedades al ViewModel
            if (producto != null)
            {
                productoVM = new ProductoVM
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    RutaImagen = producto.RutaImagen,
                    NombreImagen = producto.NombreImagen,
                    Activo = producto.Activo,
                    Categoria = new CategoriaVM
                    {
                        IdCategoria = producto.oCategoria!.IdCategoria,
                        Descripcion = producto.oCategoria.Descripcion
                    },
                    Marca = new MarcaVM
                    {
                        IdMarca = producto.oMarca!.IdMarca,
                        Descripcion = producto.oMarca.Descripcion
                    }
                };
            }
            // Cargar las listas de categorías y marcas filtradas para el dropdown sea para agregar o editar un producto
            productoVM.listaCategorias = categoriasFiltradas.Select(c => new SelectListItem
            {
                Value = c.IdCategoria.ToString(),
                Text = c.Descripcion
            }).ToList();

            productoVM.listaMarcas = marcasFiltradas.Select(m => new SelectListItem
            {
                Value = m.IdMarca.ToString(),
                Text = m.Descripcion
            }).ToList();

            return productoVM;
        }

        public async Task AddAsync(ProductoVM viewModel)
        {
            if (viewModel.ArchivoImagen != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(viewModel.ArchivoImagen.FileName);
                string extension = Path.GetExtension(viewModel.ArchivoImagen.FileName);
                viewModel.NombreImagen = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", viewModel.NombreImagen);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await viewModel.ArchivoImagen.CopyToAsync(fileStream);
                }
                viewModel.RutaImagen = "/images/" + viewModel.NombreImagen;
            }

            var entidad = new Producto
            {
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Precio = viewModel.Precio,
                Stock = viewModel.Stock,
                RutaImagen = viewModel.RutaImagen,
                NombreImagen = viewModel.NombreImagen,
                Activo = viewModel.Activo,
                IdCategoria = viewModel.Categoria.IdCategoria,
                IdMarca = viewModel.Marca.IdMarca
            };

            await _productoRepository.AddAsync(entidad);
        }

        public async Task EditAsync(ProductoVM viewModel)
        {
            var entidad = await _productoRepository.GetByIdAsync(viewModel.IdProducto);
            if (viewModel.ArchivoImagen != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(viewModel.ArchivoImagen.FileName);
                string extension = Path.GetExtension(viewModel.ArchivoImagen.FileName);
                viewModel.NombreImagen = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", viewModel.NombreImagen);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await viewModel.ArchivoImagen.CopyToAsync(fileStream);
                }
                viewModel.RutaImagen = "/images/" + viewModel.NombreImagen;
            }

            if (viewModel.ArchivoImagen != null)
            {
                entidad.RutaImagen = viewModel.RutaImagen;
                entidad.NombreImagen = viewModel.NombreImagen;
            }

            entidad.Nombre = viewModel.Nombre;
            entidad.Descripcion = viewModel.Descripcion;
            entidad.Precio = viewModel.Precio;
            entidad.Stock = viewModel.Stock;
            entidad.Activo = viewModel.Activo;
            entidad.IdCategoria = viewModel.Categoria.IdCategoria;
            entidad.IdMarca = viewModel.Marca.IdMarca;

            await _productoRepository.EditAsync(entidad);
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await _productoRepository.GetByIdAsync(id);
            await _productoRepository.DeleteAsync(entidad!);
        }

        // Método adicional para obtener el catálogo de productos con sus categorías y marcas
        public async Task<IEnumerable<ProductoVM>> GetCatalogAsync(int IdCategoria = 0, int IdMarca = 0, string buscar = "")
        {
            var conditions = new List<Expression<Func<Producto, bool>>>
            {
                x => x.Stock > 0 && x.Activo // Solo productos con stock disponible y Activos
            };

            //Filtro para Categoria: Si se recibe un IdCategoria diferente de 0, se agrega una condición para filtrar por esa categoría
            if (IdCategoria != 0)
            {
                conditions.Add(x => x.IdCategoria == IdCategoria);
            }

            //Filtro por Marca: Si se recibe un IdMarca diferente de 0, se agrega una condición para filtrar por esa marca
            if (IdMarca != 0)
            {
                conditions.Add(x => x.IdMarca == IdMarca);
            }

            if (!string.IsNullOrEmpty(buscar))
            {
                conditions.Add(x => x.Nombre.Contains(buscar) || x.Descripcion.Contains(buscar));
            }

            var includes = new Expression<Func<Producto, object>>[]
            {
                p => p.oCategoria!, // Incluir la categoría
                p => p.oMarca! // Incluir la marca
            };

            var productos = await _productoRepository.GetAllAsync(
                    conditions: conditions.ToArray(), // Pasamos las condiciones para filtrar por categoría, marca y búsqueda
                    includes: includes // Pasamos el array de includes para traer categorías y marcas
                );

            var productoVMs = productos.Select(item => new ProductoVM
            {
                IdProducto = item.IdProducto,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                RutaImagen = item.RutaImagen,
                NombreImagen = item.NombreImagen,
                Activo = item.Activo,

                //TODO : Revisar si se deben eliminar
                //Propiedades adicionales para mostrar el estado en la vista
                EstadoTexto = item.Activo ? "Activo" : "Inactivo",
                EstadoColor = item.Activo ? "bg-success" : "bg-danger",
                FechaRegistro = item.FechaRegistro
            }).ToList();

            return productoVMs;
        }
    }
}
