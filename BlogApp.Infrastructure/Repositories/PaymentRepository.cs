using BlogApp.Application.Interface.IRepositories;
using BlogApp.Infrastructure.Persistence.Contexts;

namespace BlogApp.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<PaymentRepository>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
