using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Services;
using SelectPdf;
using System.Security.Claims;
using System.Threading.Tasks;


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

        //Metodo para que el usuario pueda descargar un recibo de su orden en PDF usando SelectHtmlToPdf
        public async Task<IActionResult> DescargarReciboPDFHtmlToPDF(int id) 
        {
            //1. Obtenemos la orden por su ID (aquí deberías agregar validación para asegurarte de que la orden pertenece al usuario autenticado)
            var claimIdCliente = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (claimIdCliente == null)
            {
                return Unauthorized();
            }

            int idClienteLogueado = int.Parse(claimIdCliente);

            //2. Pasamos ambos Ids (el de la orden y el del cliente logueado) para validar que la orden pertenece al cliente
            byte[] pdfBytes = await _ordenService.GenerarReciboPdfAsync(id, idClienteLogueado);

            if(pdfBytes == null)
            {
                return NotFound("Orden no encontrada o no pertenece al cliente.");
            }

            //3. Devolvemos el PDF como un archivo descargable
            return File(pdfBytes, "application/pdf", $"Recibo_Orden_{id}.pdf");
        }
    }
}
