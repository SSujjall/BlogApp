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

        // returns internal result, not ApiResponse. Orchestrator handles the rest.
        Task<InternalOrderCancellationResultDTO> CancelOrder(string userId, int orderId);
        Task CompleteOrder(string userId, int orderId);
    }
}
