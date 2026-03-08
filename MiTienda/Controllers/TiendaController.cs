using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using MiTienda.Models;

namespace MiTienda.Controllers
{
    public class TiendaController : Controller
    {
        private readonly AppDbContext _context;
        public TiendaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDepartamentos()
        {
            var lista = await _context.Departamentos
                .Select(d => new UbigeoVM
                {
                    Id = d.IdDepartamento,
                    Descripcion = d.Descripcion
                })
                .ToListAsync();

            return Json(new { data = lista });
        }

        public async Task<IActionResult> ObtenerProvincias(string idDepartamento)
        {
            var lista = await _context.Provincias
                .Where(p => p.IdDepartamento == idDepartamento)
                .Select(p => new UbigeoVM
                {
                    Id = p.IdProvincia,
                    Descripcion = p.Descripcion
                })
                .ToListAsync();
            return Json(new { data = lista });
        }

        public async Task<IActionResult> ObtenerDistritos(string idDepartamento, string idProvincia)
        {
            var lista = await _context.Distritos
                .Where(d => d.IdDepartamento == idDepartamento && d.IdProvincia == idProvincia)
                .Select(d => new UbigeoVM
                {
                    Id = d.IdDistrito,
                    Descripcion = d.Descripcion
                })
                .ToListAsync();
            return Json(new { data = lista });
        }
    }
}
