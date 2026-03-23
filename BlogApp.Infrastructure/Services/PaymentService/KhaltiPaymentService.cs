using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;
using BlogApp.Domain.GlobalConfigs;
using Microsoft.Extensions.Options;
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

        public async Task<PaymentInitiateResponseDTO> ProcessPaymentAsync(PaymentRequestDTO dto)
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

            return new PaymentInitiateResponseDTO
            {
                RedirectUrl = mappedRes.payment_url,
                ExternalTxnId = mappedRes.pidx,
                RawResponse = apiResContent
            };
        }

        public async Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(VerifyPaymentDTO dto)
        {
            var result = await CallKhaltiCheckStatus(dto.ExternalTxnId); // here extTxnId is the pidx

            var isSuccess = result.status.Equals("Completed", StringComparison.OrdinalIgnoreCase);
            return new PaymentVerificationResponseDTO
            {
                IsSuccess = isSuccess,
                TransactionId = result.transaction_id?.ToString(),
                ExternalTxnId = result.pidx,
                Amount = (decimal)result.total_amount / 100,
                Status = result.status.ToUpper()
            };
        }

        public async Task<PaymentCheckStatusResponseDTO> CheckStatusAsync(Payments payment)
        {
            var result = await CallKhaltiCheckStatus(payment.ExternalTransactionId);
            return new PaymentCheckStatusResponseDTO
            {
                ExternalTxnId = result.pidx.ToString(),
                Status = result.status.ToUpper(),
                TotalAmount = (decimal)result.total_amount / 100,
                RefTxnId = result.transaction_id?.ToString(),
            };
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        #region Helper Methods
        private async Task<KhaltiCheckStatusResponseDTO> CallKhaltiCheckStatus(string pidx)
        {
            var reqBody = new
            {
                pidx = pidx
            };

            var json = JsonSerializer.Serialize(reqBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {_config.SecretKey}");

            var apiResponse = await _httpClient.PostAsync(_config.LookupUrl, content);
            var responseJson = await apiResponse.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<KhaltiCheckStatusResponseDTO>(responseJson);
            if (responseDto == null)
            {
                throw new ServiceException(
                    new() { { "EsewaStatusCheckError", "Failed to parse Khalti status response" } },
                    HttpStatusCode.InternalServerError
                );
            }

            return responseDto;
        }
        #endregion
    }
}
