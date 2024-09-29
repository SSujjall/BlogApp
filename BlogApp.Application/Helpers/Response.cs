using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Helpers
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
        
        //Constructor
        public Response(object? data, List<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Data = data;
            Errors = errors == null ? new List<string>() : errors;
        }
    }
}
