
namespace ECommerce510.API.Repositories.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task DeleteAllAsync(List<Cart> entities, CancellationToken cancellationToken = default);
    }
}
