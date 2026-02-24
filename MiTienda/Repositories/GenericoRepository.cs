using Microsoft.EntityFrameworkCore;
using MiTienda.Context;

namespace MiTienda.Repositories
{
    public class GenericoRepository<TEntidad>(AppDbContext _dbContext) where TEntidad : class
    {
        public async Task<IEnumerable<TEntidad>> GetAllAsync()
        {
            return await _dbContext.Set<TEntidad>().ToListAsync();
        }

        public async Task AddAsync(TEntidad entidad)
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
