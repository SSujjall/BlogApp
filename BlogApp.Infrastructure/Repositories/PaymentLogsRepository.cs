using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Contexts;

namespace BlogApp.Infrastructure.Repositories
{
    public class PaymentLogsRepository : BaseRepository<PaymentLogs>, IPaymentLogsRepository
    {
        public PaymentLogsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
