using System.Net;
namespace BlogApp.Application.Helpers
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        
        //Constructor
        public Response(object? data, Dictionary<string, string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Data = data;
            Errors = errors ?? new Dictionary<string, string>();
        }
    }
}
