using System.Collections.Generic;
using ProjectName.Core.Abstractions.Enums;

namespace ProjectName.Core.Abstractions.Wrappers
{
    public class BaseResponse 
    {
        protected BaseResponse()
        {
        }

        public BaseResponse( string? message = null)
        {
            Succeeded = true;
            Message = message;            
        }
        public ResponseStatus Status { get; set; }
        protected bool? Succeeded { get; set; } = default!;
        protected string? Message { get; set; } = default!;
        public List<string>? Errors { get; set; } = default!;
       
    }
    public class BaseResponse<T>: BaseResponse
    {
        public BaseResponse()
        {
        }

        public BaseResponse(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public BaseResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public T? Data { get; set; } = default;
    }
}