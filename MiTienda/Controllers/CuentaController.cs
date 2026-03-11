using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using System.Security.Claims;

namespace MiTienda.Controllers
{
    public class CuentaController(ClienteService _clienteService) : Controller
    {
        public IActionResult Login()
        {
            var viewModel = new LoginVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM viewModel)
        {
            // ⬇️ VALIDACIÓN: Si el modelo no es valido, se devuelve la vista con el modelo para mostrar los errores de validación
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            var found = await _clienteService.Login(viewModel);

            // EL SEMÁFORO INTELIGENTE:
            if (found.IdCliente == 0)
            {
                ViewBag.mensaje = "No se encuentra cliente con esa credenciales";
                return View();
            }
            else
            {
                //Logica para iniciar sesión
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, found.IdCliente.ToString()),
                    new Claim(ClaimTypes.Name, found.NombreCompleto),
                    new Claim(ClaimTypes.Email, found.Correo)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                    }
                );

                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Register()
        {
            var viewModel = new ClienteVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(ClienteVM viewModel)
        {
            // ⬇️ VALIDACIÓN: Si el modelo no es valido, se devuelve la vista con el modelo para mostrar los errores de validación
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                await _clienteService.Register(viewModel);
                ViewBag.mensaje = "Registro exitoso, ya puedes iniciar sesión";
                ViewBag.Class = "alert-success";
            }
            catch(Exception ex)
            {
                ViewBag.mensaje = "Error al registrar cliente: " + ex.Message;
                ViewBag.Class = "alert-danger";
            }
            
            return View();

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
