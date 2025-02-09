using System.Net;
namespace BlogApp.Application.Helpers.HelperModels
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public int? TotalCount { get; set; }

        private ApiResponse()
        {
            Errors = new Dictionary<string, string>();
        }

        public static ApiResponse<T> Success(T data, string message, HttpStatusCode statusCode = HttpStatusCode.OK, int? totalCount = null)
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                TotalCount = totalCount
            };
        }

        public static ApiResponse<T> Failed(Dictionary<string, string> errors, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode,
                Data = default
            };
        }
    }

}
