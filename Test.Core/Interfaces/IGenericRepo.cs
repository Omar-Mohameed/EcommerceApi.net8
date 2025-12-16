using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Interfaces
{
    public interface IGenericRepo<T> where T : class
    { //CRUD operations

        // Create
        Task<ErrorOr<T>> AddAsync(T entity);

        // Read all
        Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync();
        Task<ErrorOr<IReadOnlyList<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        // Read by Id
        Task<ErrorOr<T>> GetByIdAsync(int id);
        Task<ErrorOr<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        // Update
        Task<ErrorOr<T>> UpdateAsync(T entity);

        // Delete
        Task<ErrorOr<bool>> DeleteAsync(int id);

        #region a
        //// Create 
        //Task AddAsync(T entity);
        //// Read
        //Task<IReadOnlyList<T>> GetAllAsync();
        //Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes);
        //Task<T> GetByIdAsync(int id);
        //Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] Includes);
        //// Update
        //Task UpdateAsync(T entity);
        //// Delete
        //Task DeleteAsync(int id);

        #endregion
    }
}
