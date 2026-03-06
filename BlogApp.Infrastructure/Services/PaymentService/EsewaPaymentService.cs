using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.GlobalConfigs;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class EsewaPaymentService : IPaymentProvider
    {
        private readonly EsewaConfig _config;
        private readonly HttpClient _httpClient;
        public EsewaPaymentService(
            IOptions<EsewaConfig> config
            ,HttpClient httpClient
        )
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<string> ProcessPaymentAsync(PaymentRequestDTO dto)
        {
            var transactionUuid = Guid.NewGuid().ToString();
            var signature = GenerateSignature(
                dto.TotalAmount,
                transactionUuid,
                _config.ProductCode
            );

            var esewaReqModel = new EsewaRequestDTO
            {
                amount = "200",
                product_delivery_charge = "0",
                product_service_charge = "0",
                product_code = _config.ProductCode,
                signed_field_names = "total_amount,transaction_uuid,product_code",
                signature = signature,
                success_url = "https://developer.esewa.com.np/success",
                failure_url = "https://developer.esewa.com.np/failure",
                tax_amount = "0",
                total_amount = "200",
                transaction_uuid = transactionUuid
            };

            var json = JsonSerializer.Serialize(esewaReqModel);
            var keyValuePair = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var content = new FormUrlEncodedContent(keyValuePair);

            var apiResponse = await _httpClient.PostAsync(_config.InitiateUrl, content);
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new ServiceException(
                    new() { { "EsewaPaymentError", "Failed to initiate eSewa payment" } },
                    HttpStatusCode.InternalServerError
                 );
            }
            var contentType = apiResponse.Content.Headers.ContentType?.MediaType;
            string responseBody;
            if (contentType == "text/html")
            {
                // if response is HTML (like redirect URLs)
                responseBody = apiResponse.RequestMessage.RequestUri.AbsoluteUri.ToString();
            }
            else
            {
                // If response is json S
                responseBody = await apiResponse.Content.ReadAsStringAsync();
            }
            return responseBody;
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        private string GenerateSignature(decimal totalAmount, string transactionUuid, string productCode)
        {
            var message = $"total_amount=200,transaction_uuid={transactionUuid},product_code={productCode}";

            var keyBytes = Encoding.UTF8.GetBytes(_config.SecretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
