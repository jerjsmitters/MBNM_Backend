using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public abstract class GenericRepository<T>(EfDbContext context) : IGenericRepository<T> where T : class
    {
        public EfDbContext Context { get; } = context;

        public async Task<T?> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return;
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
