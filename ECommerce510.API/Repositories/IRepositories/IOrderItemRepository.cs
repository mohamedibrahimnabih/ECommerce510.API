
namespace ECommerce510.API.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task CreateAll(List<OrderItem> entities, CancellationToken cancellationToken = default);
    }
}
