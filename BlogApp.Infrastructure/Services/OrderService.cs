using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using System.Net;

namespace BlogApp.Infrastructure.Services
{
    public class OrderService(
        IOrderRepository _orderRepo,
        ISubscriptionRepository _subsRepo,
        IUserRepository _userRepo,
        IMapper _mapper
    ) : IOrderService
    {
        public async Task<ApiResponse<Orders>> CreateNewOrder(string userId, CreateOrderDTO dto)
        {
            var subscription = await _subsRepo.GetByIdAsync(dto.SubscriptionId);
            if (subscription is null || subscription.SubscriptionId == 1)
            {
                return ApiResponse<Orders>.Failed(
                    new() { { "SubscriptionNotFound", $"Invalid subscription id" } }, 
                    "Failed to create order", 
                    HttpStatusCode.BadRequest
                );
            }

            var orderModel = _mapper.Map<Orders>(dto);
            orderModel.UserId = userId;
            orderModel.Amount = subscription.Price;

            try
            {
                var result = await _orderRepo.AddAsync(orderModel);
                if (result == null)
                {
                    var errors = new Dictionary<string, string>
                    {
                        { "CreationFailed", "Error occured when adding order." }
                    };
                    return ApiResponse<Orders>.Failed(errors, "Failed to create order", HttpStatusCode.BadRequest);
                }
                await _orderRepo.SaveChangesAsync();
                return ApiResponse<Orders>.Success(result, "Order created successfully", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                var errors = new Dictionary<string, string>
                {
                    { "ExceptionOccured", ex.Message }
                };
                throw new ServiceException(errors, HttpStatusCode.InternalServerError);
            }
        }

        public Task<ApiResponse<string>> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        // For admins only
        public async Task<ApiResponse<IEnumerable<Orders>>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllAsync(null);
            return ApiResponse<IEnumerable<Orders>>.Success(orders, "Orders retrieved successfully", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<IEnumerable<Orders>>> GetOrdersByUserId(string userId)
        {
            //var userOrders = await _orderRepo.GetAllAsync(new GetRequest<Orders>
            //{
            //    Filter = o => o.UserId == userId,
            //}); This is also correct.

            var userOrders = await _orderRepo.FindAllByConditionAsync(o => o.UserId == userId);
            return ApiResponse<IEnumerable<Orders>>.Success(userOrders, "User orders retrieved successfully", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<Orders>> GetOrderById(string userId, int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order is null || order.UserId != userId)
            {
                return ApiResponse<Orders>.Failed(
                    new() { { "OrderNotFound", $"No order found." } }, 
                    "Failed to retrieve order", 
                    HttpStatusCode.NotFound
                );
            }
            return ApiResponse<Orders>.Success(order, "Order retrieved successfully", HttpStatusCode.OK);
        }

        public Task<ApiResponse<Orders>> UpdateOrder(UpdateOrderDTO dto)
        {
            throw new NotImplementedException(); // will be implemented when payment is implementd. This will be used for updating the order status after payment is successful.
        }
    }
}
