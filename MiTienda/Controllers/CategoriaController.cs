using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Context;
using MiTienda.Models;
using MiTienda.Services;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriaController(CategoriaService _categoriaService) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return View(categorias);
        }

        //Se usara la misma vista para agregar y editar, por eso se llama AddEdit
        public async Task<IActionResult> AddEdit(int id)
        {
            // Si el id es 0, se esta agregando una nueva categoria, por lo que se devuelve una vista vacia
            var categoriaVM = await _categoriaService.GetByIdAsync(id);
            return View(categoriaVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoriaVM entidadVM)
        {
            // ⬇️ VALIDACIÓN: Si el modelo no es valido, se devuelve la vista con el modelo para mostrar los errores de validación
            if (!ModelState.IsValid)
            {
                return View(entidadVM);
            }

            // EL SEMÁFORO INTELIGENTE:
            if (entidadVM.IdCategoria == 0)
            {
                // Si el ID oculto es 0, llamamos al método de crear
                await _categoriaService.AddAsync(entidadVM);
                ModelState.Clear();
                entidadVM = new CategoriaVM();
                ViewBag.mensaje = "Categoria agregada correctamente";
            }
            else
            {
                // Si el ID oculto tiene un número (ej: 5), llamamos al de editar
                await _categoriaService.EditAsync(entidadVM);
                ViewBag.mensaje = "Categoria editada correctamente";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return Json(new { 
                success = true,
                mensaje = "Categoria eliminada correctamente"
            });
        }


    }
}
