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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subsRepo;
        private readonly IMapper _mapper;
        public SubscriptionService(ISubscriptionRepository subsRepo, IMapper mapper)
        {
            _subsRepo = subsRepo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Subscriptions>> CreateNewSubscription(CreateSubscriptionDTO dto)
        {
            var subscriptionModel = _mapper.Map<Subscriptions>(dto);

            try
            {
                var result = await _subsRepo.AddAsync(subscriptionModel);
                if (result == null)
                {
                    var errors = new Dictionary<string, string>
                    {
                        { "CreationFailed", "Failed to create subscription." }
                    };
                    return ApiResponse<Subscriptions>.Failed(errors, "Failed to create subscription", HttpStatusCode.BadRequest);
                }
                await _subsRepo.SaveChangesAsync();
                return ApiResponse<Subscriptions>.Success(result, "Created new subscription model", HttpStatusCode.Created);
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

        public async Task<ApiResponse<string>> DeleteSubscription(int id)
        {
            var subscription = await _subsRepo.GetByIdAsync(id);
            if (subscription == null)
            {
                return ApiResponse<string>.Failed(new() { { "NotFound", "Subscription not found." } }, "Failed to delete subscription", HttpStatusCode.NotFound);
            }

            await _subsRepo.Delete(subscription);
            await _subsRepo.SaveChangesAsync();
            return ApiResponse<string>.Success(null, "Subscription deleted successfully", HttpStatusCode.OK);
        }

        public async Task<ApiResponse<IEnumerable<Subscriptions>>> GetAllSubscriptions()
        {
            var subscriptions = await _subsRepo.GetAllAsync(null);
            return ApiResponse<IEnumerable<Subscriptions>>.Success(subscriptions, "Fetched all subscriptions", HttpStatusCode.OK, subscriptions.Count());
        }

        public async Task<ApiResponse<Subscriptions>> UpdateSubscription(UpdateSubscriptionDTO dto)
        {
            var subscription = await _subsRepo.GetByIdAsync(dto.SubscriptionId);
            if (subscription == null)
            {
                var errors = new Dictionary<string, string>
                {
                    { "NotFound", $"Subscription with ID {dto.SubscriptionId} not found." }
                };
                return ApiResponse<Subscriptions>.Failed(errors, "Subscription not found", HttpStatusCode.NotFound);
            }

            bool isUpdated = false;
            if (dto.Name != null && subscription.Name != dto.Name)
            {
                subscription.Name = dto.Name;
                isUpdated = true;
            }
            if (dto.Price.HasValue && subscription.Price != dto.Price.Value)
            {
                subscription.Price = dto.Price.Value;
                isUpdated = true;
            }
            if (dto.DurationInMonths.HasValue && subscription.DurationInMonths != dto.DurationInMonths.Value)
            {
                subscription.DurationInMonths = dto.DurationInMonths.Value;
                isUpdated = true;
            }
            if (dto.Description != null && subscription.Description != dto.Description)
            {
                subscription.Description = dto.Description;
                isUpdated = true;
            }

            if (!isUpdated)
            {
                return ApiResponse<Subscriptions>.Failed(new() { { "UpdateError", "No changes detected" } }, "Failed to update", HttpStatusCode.BadRequest);
            }

            await _subsRepo.Update(subscription);
            await _subsRepo.SaveChangesAsync();
            return ApiResponse<Subscriptions>.Success(subscription, "Subscription updated successfully", HttpStatusCode.OK);
        }
    }
}
