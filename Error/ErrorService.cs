using System;
using System.Collections.Generic;

namespace EasyStocks.Error
{
    public class ErrorService : IErrorService
    {
        private readonly List<Error> _errorMessages = new List<Error>();

        private Error Track(Exception exception, ErrorId errorId, Severity severity, string message = "")
        {
            // if message is provided, take it, otherwise try to find a resource for the error id,
            // if it does not exist, use the error id as string (this is only for developers, user errors should always have a message)
            if (string.IsNullOrEmpty(message))
            {
                message = EasyStocksErrorStrings.ResourceManager.GetString(errorId.ToString());
                if (string.IsNullOrEmpty(message))
                    message = errorId.ToString();
            }

            var error = new Error(
                errorId,
                message,
                severity,
                exception);
            _errorMessages.Add(error);
            return error;
        }

        public void TrackWarning(Exception exception, ErrorId errorId)
        {
            var warning = Track(exception, errorId, Severity.Warning);
            WarningOccured?.Invoke(warning);

        }

        public event Action<Error> WarningOccured;
        public event Action<Error> ErrorOccured;

        public void TrackError(Exception exception, ErrorId errorId)
        {
            var error = Track(exception, errorId, Severity.Critical);
            ErrorOccured?.Invoke(error);
        } 
    }
}