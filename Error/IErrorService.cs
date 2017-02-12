using System;
using System.Net.Http.Headers;

namespace EasyStocks.Error
{
    /// <summary>
    /// used to store errors that appear in the system.
    /// </summary>
    public interface IErrorService
    {
        void TrackError(Exception exception, ErrorId errorId);
        void TrackWarning(Exception exception, ErrorId errorId);
        /// <summary>
        /// event is fired when a warning occures in the system
        /// </summary>
        event Action<Error> WarningOccured;
        /// <summary>
        /// event is fired when an error occurs in the system
        /// </summary>
        event Action<Error> ErrorOccured;
    }
}