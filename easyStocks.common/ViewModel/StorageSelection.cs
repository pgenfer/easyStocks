using EasyStocks.Storage;

namespace EasyStocks.ViewModel
{
    public class StorageSelection
    {
        public StorageSelection(string storageName, StorageType storageType)
        {
            StorageName = storageName;
            StorageType = storageType;
        }

        public string StorageName { get; }
        public StorageType StorageType { get; }
    }
}