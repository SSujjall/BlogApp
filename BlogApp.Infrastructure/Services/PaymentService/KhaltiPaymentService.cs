using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;
using BlogApp.Domain.GlobalConfigs;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class KhaltiPaymentService : IPaymentProvider
    {
        private readonly KhaltiConfig _config;
        private readonly HttpClient _httpClient;
        public KhaltiPaymentService(
            IOptions<KhaltiConfig> config,
            HttpClient httpClient
        )
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<string> ProcessPaymentAsync(PaymentRequestDTO dto)
        {
            var khaltiReq = new KhaltiRequestDTO
            {
                return_url = _config.ReturnUrl,
                website_url = _config.WebsiteUrl,
                amount = (int)(dto.TotalAmount * 100),
                purchase_order_id = dto.OrderId.ToString(),
                purchase_order_name = dto.SubscriptionName,
                //merchant_username = _config.MerchantUsername, Not NEEDED
                product_details = new List<ProductDetail>
                {
                    new ProductDetail
                    {
                        identity = Guid.NewGuid().ToString(),
                        name = dto.SubscriptionName,
                        total_price = (int)(dto.TotalAmount * 100),
                        unit_price = (int)(dto.TotalAmount * 100),
                        quantity = 1
                    }
                }
            };

            var json = JsonSerializer.Serialize(khaltiReq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {_config.SecretKey}");

            var apiResponse = await _httpClient.PostAsync(_config.InitiateUrl, content);
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new ServiceException(
                    new() { { "KhaltiPaymentError", "Failed to initiate Khalti payment" } },
                    HttpStatusCode.InternalServerError
                 );
            }

            var apiResContent = await apiResponse.Content.ReadAsStringAsync();
            var mappedRes = JsonSerializer.Deserialize<KhaltiInitiateResponseDTO>(apiResContent);

            if (mappedRes == null) // After this, we know that it is error, so need to check the error conditions according to khalti docs
            {
                throw new ServiceException(
                    new() { { "KhaltiPaymentError", "Failed to parse Khalti payment response" } },
                    HttpStatusCode.InternalServerError
                );
            }

            return mappedRes.payment_url;
        }

        public Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task<object> CheckStatusAsync(Payments payment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
