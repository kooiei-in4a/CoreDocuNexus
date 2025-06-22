using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Shared.Dto
{
    public class Result<T>
    {
        public bool IsSuccess { get; } = false;
        public T Value { get; }
        public string Message { get; } = string.Empty;

        public int StatusCode { get; } = 0;
        
        private Result(bool isSuccess, T? value, string error,int statusCode)
        {
            IsSuccess = isSuccess;
            Value = value ?? default!;
            Message = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty,0);

        public static Result<T> Failure(string error,int code) => new Result<T>(false, default, error,code);
    }
}
