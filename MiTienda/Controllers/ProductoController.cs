using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductoController(ProductoService _productoService) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var productos = await _productoService.GetAllAsync();
            return View(productos);
        }

        // 1. GET: Abre el formulario (Vacío si es nuevo, o lleno si es para editar)
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            // Le pedimos el modelo al servicio (traerá las listas sí o sí)
            var productoVM = await _productoService.GetByIdAsync(id);

            // Si mandaste un ID mayor a 0 pero no lo encontró en BD, da error
            if (id > 0 && productoVM.IdProducto == 0)
            {
                return NotFound();
            }

            // Enviamos el modelo a la vista (Nuevo o Editar, ya viene preparado)
            return View(productoVM);
        }

        // 2. POST: Guarda los datos en la base de datos (El semáforo inteligente)
        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductoVM entidadVM)
        {
            ModelState.Remove("ArchivoImagen");
            ModelState.Remove("NombreImagen");
            ModelState.Remove("RutaImagen");
            ModelState.Remove("Categoria");
            ModelState.Remove("Marca");
            ModelState.Remove("listaCategorias");
            ModelState.Remove("listaMarcas");

            if (ModelState.IsValid)
            {
                //Agregar Sanitize para la descripcion del producto (evitar caracteres especiales, espacios, etc)
                if (!string.IsNullOrEmpty(entidadVM.Descripcion))
                {
                    var sanitizer = new HtmlSanitizer();
                    entidadVM.Descripcion = sanitizer.Sanitize(entidadVM.Descripcion);
                }

                if (entidadVM.IdProducto == 0)
                {
                    await _productoService.AddAsync(entidadVM);
                    ModelState.Clear();
                    entidadVM = new ProductoVM();
                    ViewBag.mensaje = "El producto se creó correctamente.";
                }
                else
                {
                    await _productoService.EditAsync(entidadVM);
                    ViewBag.mensaje = "El producto se actualizó correctamente.";
                }

                return RedirectToAction(nameof(Index));
               
            }

            //Si hay errores de validacion y se debe retornar al formulario
            //Como las listas estan vacias, se deben volver a pedirlas a la BD
            var listasFrescas = await _productoService.GetByIdAsync(0);
            entidadVM.listaCategorias = listasFrescas.listaCategorias;
            entidadVM.listaMarcas = listasFrescas.listaMarcas;

            //Retorna la vista con los campos llenos
            return View(entidadVM);
        }

        // 3. POST (AJAX): Elimina el producto y le responde a tu SweetAlert2
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //1. Eliminamos la marca de la base de datos
                await _productoService.DeleteAsync(id);

                return Json(new { success = true, message = "Producto eliminado correctamente"});
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el producto" });
            }
        }
    }
}
