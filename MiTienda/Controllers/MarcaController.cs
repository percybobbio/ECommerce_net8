using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;

namespace MiTienda.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MarcaController(MarcaService _marcaService) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var marcas = await _marcaService.GetAllAsync();
            return View(marcas);
        }

        // 1. GET: Abre el formulario (Vacío si es nuevo, o lleno si es para editar)
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            if (id == 0)
            {
                // Si el ID es 0, es una marca nueva. Mandamos el modelo en blanco.
                return View(new MarcaVM());
            }
            else
            {
                // Si el ID es mayor a 0, buscamos la marca en la base de datos para editarla.
                var marcaVM = await _marcaService.GetByIdAsync(id);
                if (marcaVM == null)
                {
                    return NotFound();
                }
                return View(marcaVM);
            }
        }

        // 2. POST: Guarda los datos en la base de datos (El semáforo inteligente)
        [HttpPost]
        public async Task<IActionResult> AddEdit(MarcaVM entidadVM)
        {
            if (!ModelState.IsValid)
            {
                return View(entidadVM); // Si el usuario dejó algo en blanco, lo devolvemos al formulario
            }

            if (entidadVM.IdMarca == 0)
            {
                // Si es 0, CREAMOS
                await _marcaService.AddAsync(entidadVM);
                TempData["mensaje"] = "La marca se creó correctamente."; // Opcional: para mostrar un mensaje
            }
            else
            {
                // Si es mayor a 0, EDITAMOS
                await _marcaService.EditAsync(entidadVM);
                TempData["mensaje"] = "La marca se actualizó correctamente.";
            }

            // Al terminar, lo mandamos de regreso a la tabla principal
            return RedirectToAction(nameof(Index));
        }

        // 3. POST (AJAX): Elimina la marca y le responde a tu SweetAlert2
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            //1. Eliminamos la marca de la base de datos
            await _marcaService.DeleteAsync(id);

            //2. Guardamos un mensaje temporal para mostrarlo en la tabla principal (opcional)
            TempData["mensaje"] = "La marca se eliminó correctamente.";

            //3. Redirigimos al usuario de regreso a la tabla principal (Index)
            return RedirectToAction(nameof(Index));
        }
    }
}
