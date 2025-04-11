using Microsoft.EntityFrameworkCore;

namespace ECommerce510.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Edit(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }
    }
}
