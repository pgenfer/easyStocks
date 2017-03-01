using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// this base class is for view models that can be awaited.
    /// Therefore they have an additional TaskCompletionSource
    /// which is set when the view model is closed. 
    /// Code that is waiting for the view model result can then continue.
    /// </summary>
    public abstract class AwaitableViewModel : Screen
    {
        /// <summary>
        /// the view model must control the application flow
        /// so the main application should only get notified after the user has chosen
        /// a setting
        /// </summary>
        private readonly TaskCompletionSource<bool> _taskCompleteResult = new TaskCompletionSource<bool>();
        /// <summary>
        /// await this task object here if you want to wait for this view model to close.
        /// </summary>
        public Task ViewModelClosedTask => _taskCompleteResult.Task;
        /// <summary>
        /// call this method to close this view model
        /// </summary>
        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
            _taskCompleteResult.SetResult(true);
        }
    }
}
