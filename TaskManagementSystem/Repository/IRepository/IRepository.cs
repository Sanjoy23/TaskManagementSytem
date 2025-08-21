using System.Linq.Expressions;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.IRepository
{
    public interface IRepository<T> where T: class 
    {
        T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, Expression<Func<T, TResult>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<PagedResult<TResult>> GetPagedAsync<TResult>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, Expression<Func<T, TResult>>? selector = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                    int pageNumber = 1,
                    int pageSize = 10);
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
