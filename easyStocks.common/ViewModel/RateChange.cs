using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// enum used to describe the trend
    /// of a rate's change
    /// </summary>
    public enum RateChange
    {
        /// <summary>
        /// rate became negative
        /// </summary>
        Negative,
        /// <summary>
        /// rate did not change
        /// </summary>
        NoChange,
        /// <summary>
        /// rate became positive since the last time
        /// </summary>
        Positive
    }
}
