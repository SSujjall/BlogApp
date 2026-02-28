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

        public Task<ApiResponse<string>> DeleteSubscription(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<Subscriptions>>> GetAllSubscriptions()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<Subscriptions>> UpdateSubscription(UpdateSubscriptionDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
