using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.ViewModel;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace EasyStocks.View
{
    public partial class SearchShareView
    {
        private void OnSearchItemTapped(object sender, ItemTappedEventArgs e)
        {
            var searchItem = e.ItemData as ShareDailyInformation;
            if (searchItem != null)
            {
                var searchViewModel = BindingContext as SearchShareViewModel;
                searchViewModel?.SelectShare(searchItem);
            }
        }
    }
}
