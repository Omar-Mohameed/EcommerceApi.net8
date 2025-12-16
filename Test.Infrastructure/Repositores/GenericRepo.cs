using ErrorOr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositores
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<T>> AddAsync(T entity)
        {
                await _context.Set<T>().AddAsync(entity);
                return entity;
        }

        public async Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync()
        {
                var result = await _context.Set<T>().AsNoTracking().ToListAsync();
                return result;
        }

        public async Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                    query = query.Include(include);

                var list = await query.ToListAsync();
                return list;
        }

        public async Task<ErrorOr<T>> GetByIdAsync(int id)
        {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity is null)
                    return Error.NotFound(
                        code: $"{typeof(T).Name}.NotFound",
                        description: $"{typeof(T).Name} with Id {id} not found"
                    );

                return entity;
        }

        public async Task<ErrorOr<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                    query = query.Include(include);

                var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
                if (entity is null)
                    return Error.NotFound(
                        code: $"{typeof(T).Name}.NotFound",
                        description: $"{typeof(T).Name} with Id {id} not found"
                    );

                return entity;
        }

        public async Task<ErrorOr<T>> UpdateAsync(T entity)
        {
                _context.Set<T>().Update(entity);
                return entity;
        }
        public async Task<ErrorOr<bool>> DeleteAsync(int id)
        {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity is null)
                    return Error.NotFound(
                        code: $"{typeof(T).Name}.NotFound",
                        description: $"{typeof(T).Name} with Id {id} not found"
                    );

                _context.Set<T>().Remove(entity);
                return true;
        }

    }
}
