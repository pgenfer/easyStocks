using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccessful { get; }
        public string ErrorMessage { get; }

        private Result(T value)
        {
            Value = value;
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }

        private Result(T value, string errorMessage)
        {
            Value = value;
            IsSuccessful = false;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Error(T value, string error) => new Result<T>(value,error);

        public override string ToString() => IsSuccessful ? Value.ToString() : $"Error: {ErrorMessage}";
    }
}
