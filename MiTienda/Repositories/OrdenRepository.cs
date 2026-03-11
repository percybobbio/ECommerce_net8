using Microsoft.EntityFrameworkCore;
using MiTienda.CapaEntidades;
using MiTienda.Context;
using MiTienda.DTOs;

namespace MiTienda.Repositories
{
    public class OrdenRepository : GenericoRepository<Venta>
    {
        private readonly AppDbContext _dbContext;
        public OrdenRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //Utilizamos override para modificar el comportamiento del método AddAsync en la clase base (GenericoRepository) para que se adapte a las necesidades específicas de la entidad Venta.
        //Esto nos permite agregar lógica adicional o realizar acciones específicas antes o después de llamar al método base, como validar los datos de la venta, actualizar el inventario, o enviar notificaciones, asegurando que el proceso de agregar una venta se maneje de manera adecuada y eficiente.
        public override async Task AddAsync(Venta entidad)
        {
            //usamos transacciones para que se ejecute todo o nada, es decir, si ocurre un error en alguna parte del proceso, se revertirán todos los cambios realizados hasta ese punto, manteniendo la integridad de los datos.
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var detalle in entidad.oDetalleVenta)
                {
                    var producto = await _dbContext.Productos.FindAsync(detalle.IdProducto);
                    if (producto != null)
                    {
                        if (producto.Stock >= detalle.Cantidad)
                        {
                            producto.Stock -= detalle.Cantidad;
                            _dbContext.Productos.Update(producto);
                        }
                        else
                        {
                            throw new Exception($"No hay suficiente stock para el producto {producto.Nombre}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Producto con ID {detalle.IdProducto} no encontrado");
                    }
                }

                //Agregamos la venta primero para generar el IdVenta, que es necesario para la relación con la dirección
                await _dbContext.Ventas.AddAsync(entidad);
                await _dbContext.SaveChangesAsync(); //Genera el Id de la venta para que se pueda usar en los detalles y la direccion

                //Commit es para guardar los cambios
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Si ocurre un error regresa los cambios
                await transaction.RollbackAsync();

                // Capturamos el error real de la base de datos
                string errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                // Lanzamos la excepción con el mensaje real
                throw new Exception($"Error en la Base de Datos: {errorReal}");
            }
        }

        public async Task<IEnumerable<Venta>> ObtenerVentasConDetalleAsync(int idCliente)
        {
            var ventas = await _dbContext.Ventas
                .Where(v => v.IdCliente == idCliente)
                .OrderByDescending(v => v.FechaVenta)
                .Include(v => v.oDetalleVenta)
                    .ThenInclude(dv => dv.oProducto)
                .Include(v => v.oDireccion)
                    .ThenInclude(d => d.oDistrito) //carga distrito
                        .ThenInclude(dis => dis.oProvincia) //Desde Distrito Carga Provincia
                        .ThenInclude(pro => pro.oDepartamento) //Desde Provincia Carga Departamento
                .ToListAsync();

            return ventas;
        }
    }
}
