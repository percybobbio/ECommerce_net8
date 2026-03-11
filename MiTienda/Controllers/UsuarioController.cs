using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Services;
using System.Security.Claims;


namespace MiTienda.Controllers
{
    [Authorize]
    public class UsuarioController(OrdenService _ordenService) : Controller
    {
        public async Task<IActionResult> MyOrders()
        {
            //HECHO: cambiar id
            var idcliente = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var ordenes = await _ordenService.ObtenerOrdenesPorClienteAsync(int.Parse(idcliente));
            return View(ordenes);
        }
    }
}
