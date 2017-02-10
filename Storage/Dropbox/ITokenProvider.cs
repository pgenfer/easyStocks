using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyStocks.Storage.Dropbox
{
    public interface ITokenProvider
    {
        string Token { get; }
    }
}
