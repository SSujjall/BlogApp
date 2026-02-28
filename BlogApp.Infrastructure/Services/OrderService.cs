using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;

namespace BlogApp.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        public Task<ApiResponse<object>> CreateOrder(CreateOrderDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
