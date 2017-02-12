using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Error;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// view model is used for visualizing
    /// error or warning messages on screen. 
    /// The messages disappear after a specific time automatically.
    /// </summary>
    public class ErrorViewModel : PropertyChangedBase
    {
        private bool _hasMessage;
        private string _message;
        private Severity _severity;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(5);
        private readonly Timer _showErrorMessageTimer;


        public ErrorViewModel(IErrorService errorService)
        {
            errorService.WarningOccured += WarningOrErrorOccured;
            errorService.ErrorOccured += WarningOrErrorOccured;

            _showErrorMessageTimer = new Timer(
                OnShowErrorMessageTimerElapsed,
                null,
                Timeout.InfiniteTimeSpan,
                Timeout.InfiniteTimeSpan);
        }

        private void OnShowErrorMessageTimerElapsed(object state)
        {
            _showErrorMessageTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            HasMessage = false;
        }

        private void WarningOrErrorOccured(Error.Error error)
        {
            Message = error.Message;
            Severity = error.Severity;
            HasMessage = true;
        }

        /// <summary>
        /// flag is set when a message appears and
        /// is reset when the message should disappear.
        /// </summary>
        public bool HasMessage
        {
            get { return _hasMessage; }
            private set
            {
                if (value == _hasMessage) return;
                _hasMessage = value;
                NotifyOfPropertyChange();
                // message should be hidden after interval elapsed
                _showErrorMessageTimer.Change(_updateInterval, Timeout.InfiniteTimeSpan);
            }
        }

        public Severity Severity
        {
            get { return _severity; }
            private set
            {
                if (value == _severity) return;
                _severity = value;
                NotifyOfPropertyChange();
            }
        }

        public string Message
        {
            get { return _message; }
            private set
            {
                if (value == _message) return;
                _message = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
