using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using TaskManagementSystem.Data;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, Expression<Func<T, TResult>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (selector != null)
            {
                return query.Select(selector).ToList();
            }

            return query.Cast<TResult>().ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;

            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }

            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public async Task<PagedResult<TResult>> GetPagedAsync<TResult>(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            Expression<Func<T, TResult>>? selector = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<T> query = _dbSet;

            // Apply filter first to reduce dataset
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Get total count before pagination (important for UI)
            var totalCount = await query.CountAsync();

            // Apply includes
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            // Apply ordering
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Apply projection and execute query
            List<TResult> data;
            if (selector != null)
            {
                data = await query.Select(selector).ToListAsync();
            }
            else
            {
                data = await query.Cast<TResult>().ToListAsync();
            }

            return new PagedResult<TResult>(data, pageNumber, pageSize, totalCount);
        }
    }
}
