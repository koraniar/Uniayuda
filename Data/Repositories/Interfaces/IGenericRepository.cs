using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAllInclude(string path);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(Guid Id);
        Task<T> GetByIdAsync(Guid Id);
        T GetSingleOrDefault(Expression<Func<T, bool>> match);
        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> match);
        T GetFirstOrDefault(Expression<Func<T, bool>> match, bool? reloadEntity = false);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> match);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> match);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> match);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Func<T, Boolean> predicate);
        int Count();
        Task<int> CountAsync();
    }
}
