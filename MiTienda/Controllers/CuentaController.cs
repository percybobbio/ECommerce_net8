using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Models;
using MiTienda.Services;
using System.Security.Claims;

namespace MiTienda.Controllers
{
    public class CuentaController(
        ClienteService _clienteService,
        UsuarioService _usuarioService
        ) : Controller
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

            // ==================================================
            // PASO 1: ¿Es un Administrador? (Tabla Usuario)
            // ==================================================
            var foundAdmin = await _usuarioService.Login(viewModel);

            //Si el usuario es diferente de 0 significa que encontró un usuario con esas credenciales
            if (foundAdmin != null && foundAdmin.IdUsuario != 0)
            {
                await CrearSesionAutenticacion(
                    foundAdmin.IdUsuario.ToString(),
                    foundAdmin.NombreCompleto,
                    foundAdmin.Correo,
                    "Admin" //Asignamos rol de Admin
                    );

                return RedirectToAction("Index", "Home");
            }

            // ==================================================
            // PASO 2: Si no es Admin, ¿Es un Cliente? (Tabla Cliente)
            // ==================================================

            var foundClient = await _clienteService.Login(viewModel);

            // EL SEMÁFORO INTELIGENTE:
            if (foundClient != null && foundClient.IdCliente != 0)
            {
                await CrearSesionAutenticacion(
                    foundClient.IdCliente.ToString(),
                    foundClient.NombreCompleto,
                    foundClient.Correo,
                    "Cliente" //Asignamos rol de Cliente
                    );

                return RedirectToAction("Index", "Home");
            }

            // ==================================================
            // PASO 3: Credenciales incorrectas en ambas tablas
            // ==================================================

            ViewBag.mensaje = "No se encuentra cliente con esa credenciales";
            return View();
        }

        //Metodo privado para no repetir codigo al crear la cookie para iniciar sesión
        private async Task CrearSesionAutenticacion(string id, string nombreCompleto, string correo, string rol)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, nombreCompleto),
                    new Claim(ClaimTypes.Email, correo),
                    new Claim(ClaimTypes.Role, rol)
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
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error al registrar cliente: " + ex.Message;
                ViewBag.Class = "alert-danger";
            }

            return View();

        }

        public async Task<IActionResult> Logout()
        {
            // Eliminar la cookie de autenticación para cerrar sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //Perfil de la cuenta
        public async Task<IActionResult> Profile()
        {
            //1. Obtenemos el ID del cliente logueado a través de las claims
            var claimIdCliente = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (claimIdCliente == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int idClienteLogueado = int.Parse(claimIdCliente);

            //2. Usamos repositorio generico para buscar el cliente por su ID y mostrar su información en la vista
            var clienteVM = await _clienteService.GetByIdAsync(idClienteLogueado);

            if (clienteVM == null)
            {
                return NotFound();
            }

            var PerfilVM = new PerfilVM
            {
                Nombres = clienteVM.Nombres,
                Apellidos = clienteVM.Apellidos,
                Correo = clienteVM.Correo
            };

            return View(PerfilVM);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(PerfilVM viewModel)
        {
            var claimIdCliente = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (claimIdCliente == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int idClienteLogueado = int.Parse(claimIdCliente);

            //Llamar al servicio para actualizar el perfil del cliente
            try
            {
                await _clienteService.EditAsync(int.Parse(claimIdCliente), viewModel);
                ViewBag.mensaje = "Perfil actualizado correctamente";
                ViewBag.Class = "alert-success";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error al actualizar perfil: " + ex.Message;
                ViewBag.Class = "alert-danger";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
