using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse<object>> CreateOrder(CreateOrderDTO dto);
    }
}
