using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;

namespace BlogApp.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        public Task<ApiResponse<Orders>> CreateOrder(CreateOrderDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<Orders>>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Orders>> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Orders>> UpdateOrder(UpdateOrderDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
