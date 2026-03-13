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
                c => c.Correo == loginVM.Correo
            };

            var clienteEncontrado = await _userRepository.GetByFilter(conditions: conditions.ToArray());

            var clienteVM = new ClienteVM();

            //Si existe el correo verificamos la contraseña usando BCrypt
            if (clienteEncontrado != null && BCrypt.Net.BCrypt.Verify(loginVM.Clave, clienteEncontrado.Clave))
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
                Clave = BCrypt.Net.BCrypt.HashPassword(clienteVM.Clave), //Hasheamos la contraseña antes de guardarla
                Reestablecer = clienteVM.Reestablecer,
                FechaRegistro = DateTime.Now.ToUniversalTime()
            };

            await _userRepository.AddAsync(entidad);
        }

        public async Task<ClienteVM> GetByIdAsync(int id)
        {
            var cliente = await _userRepository.GetByIdAsync(id);
            var clienteVM = new ClienteVM();
            if(cliente != null)
            {
                clienteVM.IdCliente = cliente.IdCliente;
                clienteVM.Nombres = cliente.Nombres;
                clienteVM.Apellidos = cliente.Apellidos;
                clienteVM.Correo = cliente.Correo;
                clienteVM.Reestablecer = cliente.Reestablecer;
            }
            return clienteVM;
        }

        public async Task EditAsync(int idCliente, PerfilVM viewModel)
        {
            var entidad = await _userRepository.GetByIdAsync(idCliente);
            if(entidad != null)
            {
                entidad.Nombres = viewModel.Nombres;
                entidad.Apellidos =viewModel.Apellidos;

                await _userRepository.EditAsync(entidad);
            }
        }
    }
}
