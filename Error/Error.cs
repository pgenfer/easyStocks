using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Error
{
    public class Error
    {
        private readonly Exception _exception;

        public Error(
            ErrorId  id, 
            string message = "", 
            Severity severity = Severity.Critical, 
            Exception exception = null)
        {
            Id = id;
            Message = message;
            Severity = severity;
            _exception = exception;
        }

        public Severity Severity { get; }
        public string Message { get; }
        public ErrorId Id { get; }
    }
}
