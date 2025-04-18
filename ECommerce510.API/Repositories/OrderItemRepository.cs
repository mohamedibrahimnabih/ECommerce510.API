using System.Threading.Tasks;

namespace ECommerce510.API.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task CreateAll(List<OrderItem> entities, CancellationToken cancellationToken = default)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
