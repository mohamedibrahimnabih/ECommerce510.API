namespace ECommerce510.API.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public void Create(T entity);
        public void Edit(T entity);
        public void Delete(T entity);
        public void Commit();
    }
}
