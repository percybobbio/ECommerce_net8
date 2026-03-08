using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using System.Linq.Expressions;

namespace MiTienda.Repositories
{
    public class GenericoRepository<TEntidad>(AppDbContext _dbContext) where TEntidad : class
    {
        public async Task<IEnumerable<TEntidad>> GetAllAsync()
        {
            return await _dbContext.Set<TEntidad>().ToListAsync();
        }

        //Generico para obtener una entidad relacionada a otra, por ejemplo, obtener un producto con su categoría
        public async Task<IEnumerable<TEntidad>> GetAllAsync(
            Expression<Func<TEntidad, bool>>[]? conditions = null,
            Expression<Func<TEntidad, object>>[]? includes = null
            )
        {
            IQueryable<TEntidad> query = _dbContext.Set<TEntidad>();

            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    query = query.Where(condition);
                }
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public virtual async Task AddAsync(TEntidad entidad)
        {
            _dbContext.Set<TEntidad>().Add(entidad);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntidad?> GetByIdAsync(int entidadId)
        {
            return await _dbContext.Set<TEntidad>().FindAsync(entidadId);
        }

        public async Task EditAsync(TEntidad entidad)
        {
            _dbContext.Set<TEntidad>().Update(entidad);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntidad entidad)
        {
            _dbContext.Set<TEntidad>().Remove(entidad);
            await _dbContext.SaveChangesAsync();
        }
    }
}
