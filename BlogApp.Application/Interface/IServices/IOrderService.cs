using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse<Orders>> CreateOrder(CreateOrderDTO dto);
        Task<ApiResponse<IEnumerable<Orders>>> GetAllOrders();
         Task<ApiResponse<Orders>> GetOrderById(int id);
         Task<ApiResponse<Orders>> UpdateOrder(UpdateOrderDTO dto);
         Task<ApiResponse<string>> DeleteOrder(int id);
    }
}
