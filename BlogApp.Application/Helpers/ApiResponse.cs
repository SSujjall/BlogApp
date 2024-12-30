using System.Net;
namespace BlogApp.Application.Helpers
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        private ApiResponse()
        {
            Errors = new Dictionary<string, string>();
        }

        public static ApiResponse<T> Success(T data, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
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
