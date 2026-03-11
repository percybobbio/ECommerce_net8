using MiTienda.CapaEntidades;
using MiTienda.Models;
using MiTienda.Repositories;
using System.Linq.Expressions;

namespace MiTienda.Services
{
    public class ClienteService(GenericoRepository<Cliente> _userRepository)
    {
        public async Task<ClienteVM> Login(LoginVM loginVM)
        {
            var conditions = new List<Expression<Func<Cliente, bool>>>()
            {
                c => c.Correo == loginVM.Correo,
                c => c.Clave == loginVM.Clave
            };

            var clienteEncontrado = await _userRepository.GetByFilter(conditions: conditions.ToArray());

            var clienteVM = new ClienteVM();

            if(clienteEncontrado != null)
            {
                clienteVM.IdCliente = clienteEncontrado.IdCliente;
                clienteVM.Nombres = clienteEncontrado.Nombres;
                clienteVM.Apellidos = clienteEncontrado.Apellidos;
                clienteVM.Correo = clienteEncontrado.Correo;
                clienteVM.Reestablecer = clienteEncontrado.Reestablecer;
            }
            
            return clienteVM;            
        }

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
    }
}
