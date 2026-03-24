using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using System.Net;

namespace BlogApp.Infrastructure.Services
{
    public class OrderService(
        IOrderRepository _orderRepo,
        ISubscriptionRepository _subsRepo,
        IMapper _mapper,
        ITransactionService _txnService
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

        public async Task<InternalOrderCancellationResultDTO> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepo.FindSingleByConditionAsync(
                o => o.OrderId == orderId
                        && o.UserId == userId
                        && o.Status != OrderStatus.Completed
                        && o.Status != OrderStatus.Canceled
            );

            if (order == null)
            {
                throw new ServiceException(
                    new() { { "OrderNotFound", "Order not found or already processed" } },
                    HttpStatusCode.NotFound
                );
            }

            // Update order
            order.Status = OrderStatus.Canceled;
            order.UpdatedAt = DateTime.UtcNow;

            return new InternalOrderCancellationResultDTO
            {
                OrderId = orderId,
                UserId = userId
            };
        }

        public async Task CompleteOrder(string userId, int orderId)
        {
            var order = await _orderRepo.FindSingleByConditionAsync(
                o => o.OrderId == orderId && o.UserId == userId
            );

            if (order is null || order.Status == OrderStatus.Canceled)
            {
                throw new ServiceException(
                    new() { { "OrderStatusError", "Order not found or cannot be completed" } },
                    HttpStatusCode.BadRequest
                );
            }

            // Idempotent — already completed is fine, orchestrator handles this via AlreadyVerified
            if (order.Status == OrderStatus.Completed) return;

            order.Status = OrderStatus.Completed;

            // Note: SaveChangesAsync not called here — transaction in orchestrator commits everything at once
        }
    }
}
