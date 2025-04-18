namespace ECommerce510.API.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task DeleteAllAsync(List<Cart> entities, CancellationToken cancellationToken = default)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
