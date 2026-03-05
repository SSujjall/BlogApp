using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse<Orders>> CreateNewOrder(string userId, CreateOrderDTO dto);
        Task<ApiResponse<IEnumerable<Orders>>> GetAllOrders();
        Task<ApiResponse<IEnumerable<Orders>>> GetOrdersByUserId(string userId);
        Task<ApiResponse<Orders>> GetOrderById(string userId, int id);
        Task<ApiResponse<Orders>> UpdateOrder(UpdateOrderDTO dto);
        Task<ApiResponse<string>> DeleteOrder(int id);
    }
}
