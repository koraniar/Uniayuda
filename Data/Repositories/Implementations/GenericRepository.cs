using Data.DBInteractions;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DatabaseContext _dataContext;
        private readonly DbSet<T> _dbset;

        public GenericRepository(IDBFactory databaseFactory)
        {
            _dataContext = databaseFactory.Get();
            _dbset = _dataContext.Set<T>();
        }

        public virtual IEnumerable<T> GetAllInclude(string path)
        {
            return _dbset.Include(path).ToList();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public virtual T GetById(Guid id)
        {
            return _dbset.Find(id);
        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbset.FindAsync(id);
        }
        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> match)
        {
            return _dbset.SingleOrDefault(match);
        }
        public virtual async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> match)
        {
            return await _dbset.SingleOrDefaultAsync(match);
        }
        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> match, bool? reloadEntity = false)
        {
            T entity = _dbset.FirstOrDefault(match);
            if (reloadEntity.HasValue && reloadEntity == true)
                _dataContext.Entry(entity).Reload();
            return entity;
        }
        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> match)
        {
            return await _dbset.FirstOrDefaultAsync(match);
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> match)
        {
            return _dbset.Where(match).ToList();

        }
        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> match)
        {
            return await _dbset.Where(match).ToListAsync();
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public virtual void Delete(Func<T, Boolean> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }

        public virtual int Count()
        {
            return _dbset.Count();
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbset.CountAsync();
        }
    }
}
