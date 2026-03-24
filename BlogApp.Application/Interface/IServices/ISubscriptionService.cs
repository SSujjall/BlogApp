using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices
{
    public interface ISubscriptionService
    {
        Task<ApiResponse<Subscriptions>> CreateNewSubscription(CreateSubscriptionDTO dto);
        Task<ApiResponse<IEnumerable<Subscriptions>>> GetAllSubscriptions();
         Task<ApiResponse<Subscriptions>> UpdateSubscription(UpdateSubscriptionDTO dto);
         Task<ApiResponse<string>> DeleteSubscription(int id);
    }
}
