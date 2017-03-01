using System;

namespace EasyStocks.Error
{
    /// <summary>
    /// this is a just a "noop" error service, it does not track the errors but just rethrows them.
    /// In that way, an error service can be injected into a component but the errors can still be propagated to other components.
    /// This technique is used when storages are composed (by a default and a fallback service).
    /// </summary>
    public class ThrowExceptionErrorService : IErrorService
    {
        public void TrackError(Exception exception, ErrorId errorId)
        {
            throw exception;
        }

        public void TrackWarning(Exception exception, ErrorId errorId)
        {
            throw exception;
        }

        public event Action<Error> WarningOccured;
        public event Action<Error> ErrorOccured;
    }
}