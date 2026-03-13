using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;
using System.Linq.Expressions;

namespace MiTienda.Services
{
    public class UsuarioService(GenericoRepository<Usuario> _adminRepository)
    {
        public async Task<UsuarioVM> Login(LoginVM loginVM)
        {
            var conditions = new List<Expression<Func<Usuario, bool>>>()
            {
                c => c.Correo == loginVM.Correo
            };

            var adminEncontrado = await _adminRepository.GetByFilter(conditions: conditions.ToArray());

            var usuarioVM = new UsuarioVM();

            if (adminEncontrado != null && BCrypt.Net.BCrypt.Verify(loginVM.Clave, adminEncontrado.Clave))
            {
                usuarioVM.IdUsuario = adminEncontrado.IdUsuario;
                usuarioVM.Nombres = adminEncontrado.Nombres;
                usuarioVM.Apellidos = adminEncontrado.Apellidos;
                usuarioVM.Correo = adminEncontrado.Correo;
                usuarioVM.Reestablecer = adminEncontrado.Reestablecer;
                usuarioVM.Activo = adminEncontrado.Activo;
            }
            
            return usuarioVM;            
        }
        /* Verificar registro de usuarios luego
        public async Task Register(ClienteVM clienteVM)
        {
            if(clienteVM.Clave != clienteVM.ConfirmarClave)
            {
                throw new InvalidOperationException("Las contraseñas no coinciden.");
            }

            var conditions = new List<Expression<Func<Cliente, bool>>>()
            {
                c => c.Correo == clienteVM.Correo
            };

            var clienteExistente = await _userRepository.GetByFilter(conditions: conditions.ToArray());

            if(clienteExistente != null)
            {
                throw new InvalidOperationException("El correo ya está registrado.");
            }

            var entidad = new Cliente()
            {
                Nombres = clienteVM.Nombres,
                Apellidos = clienteVM.Apellidos,
                Correo = clienteVM.Correo,
                Clave = clienteVM.Clave,
                Reestablecer = clienteVM.Reestablecer,
                FechaRegistro = DateTime.Now.ToUniversalTime()
            };

            await _userRepository.AddAsync(entidad);
        }
        */
    }
}
