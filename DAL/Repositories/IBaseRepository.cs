using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IBaseRepository<T,TId> where T : class
    {
        Task<IEnumerable<T>> FindAll(int PageNumber,int Count);
        Task<T> FindById(TId k);
        Task<T> Create(T entity);
        Task<T> Delete(TId k);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
    public abstract class BaseRepository<T,TId> : IBaseRepository<T,TId> where T : class
    {
        protected readonly DbContext RepositoryContext;
        public BaseRepository(DbContext context)
        {
            RepositoryContext = context;
        }
        public async Task<IEnumerable<T>> FindAll(int PageNumber,int Count)=>
              await RepositoryContext.Set<T>().Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        

        public async Task<T> Create(T t)
        {
            await RepositoryContext.Set<T>().AddAsync(t);
            await RepositoryContext.SaveChangesAsync();
            return t;
        }
        public async Task<T> Delete(TId id)
        {
            var result = await FindById(id);
            if (result == null) return null;
            RepositoryContext.Remove(result);
            await RepositoryContext.SaveChangesAsync();
            return result;
        }

        public void SaveChanges()
        {
            RepositoryContext.SaveChanges();
        }

        public async Task<T> FindById(TId id)
        {
            var result = RepositoryContext.Set<T>().Find(id);
            if (result == null) return null;
            return result;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => RepositoryContext.Set<T>().Where(predicate);

    }
}
