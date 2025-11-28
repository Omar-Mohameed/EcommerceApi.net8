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
            try
            {
                var result = await _context.Set<T>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.AddFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity is null)
                    return Error.NotFound(code: $"{typeof(T).Name}.NotFound", description: $"{typeof(T).Name} with Id {id} not found");

                _context.Set<T>().Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.DeleteFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync()
        {
            try
            {
                var result = await _context.Set<T>().AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.GetAllFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                    query = query.Include(include);

                var list = await query.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.GetAllFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity is null)
                    return Error.NotFound(code: $"{typeof(T).Name}.NotFound", description: $"{typeof(T).Name} with Id {id} not found");

                return entity;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.GetByIdFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                    query = query.Include(include);
                var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
                if (entity is null)
                    return Error.NotFound(code: $"{typeof(T).Name}.NotFound", description: $"{typeof(T).Name} with Id {id} not found");

                return entity;
            }
            catch (Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.GetByIdFailed", description: ex.Message);
            }
        }

        public async Task<ErrorOr<T>> UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                return entity;
            }
            catch(Exception ex)
            {
                return Error.Failure(code: $"{typeof(T).Name}.UpdateFailed", description: ex.Message);
            }
        }


        #region a
        //public async Task AddAsync(T entity)
        //{
        //    await _context.Set<T>().AddAsync(entity);
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var entity = await _context.Set<T>().FindAsync(id);
        //    if (entity != null)
        //    {
        //        _context.Set<T>().Remove(entity);
        //    }
        //}

        //public async Task<IReadOnlyList<T>> GetAllAsync()
        //{
        //    return await _context.Set<T>().AsNoTracking().ToListAsync();
        //}

        //public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes)
        //{
        //    var query = _context.Set<T>().AsQueryable();
        //    foreach (var include in Includes)
        //    {
        //        query = query.Include(include);
        //    }
        //    return await query.ToListAsync();
        //}

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    var entity =await _context.Set<T>().FindAsync(id);
        //    return entity;
        //}

        //public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] Includes)
        //{
        //    var query = _context.Set<T>().AsQueryable();
        //    foreach(var include in Includes)
        //    {
        //        query = query.Include(include);
        //    }
        //    return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        //}

        //public Task UpdateAsync(T entity)
        //{
        //    _context.Entry(entity).State = EntityState.Modified;
        //    return Task.CompletedTask;
        //}

        #endregion
    }
}
