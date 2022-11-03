using System.Collections.Generic;
using System.Net;
using ProjectName.Auth.Application.Enums;

namespace ProjectName.Auth.Application.Wrappers
{
    public class Response 
    {
        public Response()
        {
        }

        public Response( string? message = null)
        {
            Succeeded = true;
            Message = message;            
        }
        public ResponseStatus Status { get; set; }
        public bool? Succeeded { get; set; } = default!;
        public string? Message { get; set; } = default;
        public List<string>? Errors { get; set; } = default;
       
    }
    public class Response<T>: Response
    {
        public Response()
        {
        }

        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public T? Data { get; set; } = default;
    }
}