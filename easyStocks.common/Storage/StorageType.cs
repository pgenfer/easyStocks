using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Storage
{
    /// <summary>
    /// stores the type of storage that is used 
    /// to store the portfolio data.
    /// Maybe more types (like google drive etc...) will follow
    /// </summary>
    public enum StorageType
    {
        Local = 0,
        DropBox = 1
    }
}
