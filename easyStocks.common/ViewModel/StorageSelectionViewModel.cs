using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Commands;
using EasyStocks.Settings;
using EasyStocks.Storage;

namespace EasyStocks.ViewModel
{
    public class StorageSelectionViewModel : AwaitableViewModel
    {
        private ApplicationSettings _applicationSettings;
        private StorageSelection _selectedItem;
       
        public StorageSelectionViewModel()
        {
            DisplayName = EasyStocksStrings.ChooseStorageTitle;
        }

        protected override void OnActivate()
        {
            // reset the selection every time the page is loading
            SelectedItem = null;
        }

        public StorageSelection SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange();
                if(value != null)
                   OnSelectedItemChanged(value);
            }
        }

        public BindableCollection<StorageSelection> Items { get; } = new BindableCollection<StorageSelection>
        {
            new StorageSelection(EasyStocksStrings.Dropbox,StorageType.DropBox),
            new StorageSelection(EasyStocksStrings.LocalDevice,StorageType.Local)
        };

        /// <summary>
        /// called by caliburn during creation
        /// </summary>
        public ApplicationSettings Parameter
        {
            set { _applicationSettings = value; }
        }

        private void OnSelectedItemChanged(StorageSelection selection)
        {
            _applicationSettings.StorageType = selection.StorageType;
            // everybody who is waiting for the user to select 
            // the storage can not continue
            TryClose();
        }
    }
}
