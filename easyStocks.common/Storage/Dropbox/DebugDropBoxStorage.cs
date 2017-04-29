using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Error;

namespace EasyStocks.Storage.Dropbox
{
    /// <summary>
    /// debug implementation of the drop box storage uses a different file name for
    /// storing the portfolio
    /// </summary>
    public class DebugDropBoxStorage : DropBoxStorage
    {
        public DebugDropBoxStorage(string token, IErrorService errorService) : base(token, errorService)
        {
        }

        protected override string StorageFileName => "easystocks.debug.json";
    }
}
