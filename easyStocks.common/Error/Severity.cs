using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Error
{
    /// <summary>
    /// defines the level of an error.
    /// Depending on the level, the system can have a backup
    /// strategy or can ask the user for help.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// an error occured, but the system has a backup 
        /// strategy that can be used until the problem disappears.
        /// (e.g. no DropBox connection => use local storage)
        /// </summary>
        Warning,
        /// <summary>
        /// a critical error occured and the system cannot do anything
        /// to compensate.
        /// Example: No network connection => no stock data can be retrieved
        /// </summary>
        Critical
    }
}
