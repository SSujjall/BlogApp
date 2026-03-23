using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
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
            IOptions<EsewaConfig> config,
            HttpClient httpClient
        )
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<PaymentInitiateResponseDTO> ProcessPaymentAsync(PaymentRequestDTO dto)
        {
            var message = $"total_amount={dto.TotalAmount},transaction_uuid={dto.ExternalTransactionId},product_code={_config.ProductCode}";
            var signature = GenerateSignature(message);

            var esewaReqModel = new EsewaRequestDTO
            {
                amount = dto.TotalAmount.ToString(),
                product_delivery_charge = "0",
                product_service_charge = "0",
                product_code = _config.ProductCode,
                signed_field_names = "total_amount,transaction_uuid,product_code",
                signature = signature,
                success_url = _config.SuccessUrl,
                failure_url = _config.FailureUrl,
                tax_amount = "0",
                total_amount = dto.TotalAmount.ToString(),
                transaction_uuid = dto.ExternalTransactionId
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
                // If response is json
                responseBody = await apiResponse.Content.ReadAsStringAsync();
            }

            return new PaymentInitiateResponseDTO
            {
                RedirectUrl = responseBody,
                ExternalTxnId = dto.ExternalTransactionId, 
                RawResponse = await apiResponse.Content.ReadAsStringAsync()
            };
        }

        public async Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            string decodedString = Encoding.UTF8.GetString(bytes);

            var apiRes = JsonSerializer.Deserialize<EsewaResponseDTO>(decodedString);
            if (apiRes == null)
            {
                throw new ServiceException(
                    new() { { "InvalidResponse", "Invalid eSewa response data" } },
                    HttpStatusCode.BadRequest
                );
            }

            var message =
                $"transaction_code={apiRes.transaction_code}," +
                $"status={apiRes.status}," +
                $"total_amount={apiRes.total_amount}," +
                $"transaction_uuid={apiRes.transaction_uuid}," +
                $"product_code={apiRes.product_code}," +
                $"signed_field_names={apiRes.signed_field_names}";

            var generatedSignature = GenerateSignature(message);
            if (generatedSignature != apiRes.signature)
            {
                throw new ServiceException(
                    new() { { "SignatureMismatch", "Invalid eSewa payment response" } },
                    HttpStatusCode.BadRequest
                );
            }

            return new PaymentVerificationResponseDTO
            {
                IsSuccess = apiRes.status.ToUpper() == EsewaResStatus.COMPLETE.ToString(),
                TransactionId = apiRes.transaction_code,
                TransactionUuid = apiRes.transaction_uuid,
                Amount = decimal.TryParse(apiRes.total_amount, out decimal amount) ? amount : 0,
                Status = apiRes.status,
                RawResponse = decodedString
            };
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentCheckStatusResponseDTO> CheckStatusAsync(Payments payment)
        {
            var parameters = 
                $"product_code={_config.ProductCode}&" +
                $"total_amount={payment.Amount}&" +
                $"transaction_uuid={payment.ExternalTransactionId}";

            var statusCheckUrl = $"{_config.StatusCheckUrl}/?{parameters}";

            var apiResponse = await _httpClient.GetAsync(statusCheckUrl);
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new ServiceException(
                    new() { { "EsewaStatusCheckError", "Failed to status check the payment" } },
                    HttpStatusCode.InternalServerError
                 );
            }
            var json = await apiResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EsewaCheckStatusResponseDTO>(json);
            return new PaymentCheckStatusResponseDTO
            {
                ExternalTxnId = result.transaction_uuid.ToString(),
                Status = result.status.ToUpper(),
                TotalAmount = result.total_amount,
                RefTxnId = result.ref_id.ToString(),
            };
        }

        private string GenerateSignature(string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_config.SecretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
