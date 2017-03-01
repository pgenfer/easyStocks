using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Dto;

namespace EasyStocks.Storage
{
    /// <summary>
    /// marker interface to register the platform dependent file
    /// storage. In that way, the storage composition can be done in 
    /// the platform independent code by retrieving this interface from DI container.
    /// </summary>
    public interface IFileSystemStorage : IStorage
    {
    }
}
