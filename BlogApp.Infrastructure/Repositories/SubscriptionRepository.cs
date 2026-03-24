using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Contexts;

namespace BlogApp.Infrastructure.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscriptions>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
