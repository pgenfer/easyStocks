using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EasyStocks.Model;
using EasyStocks.ViewModel;

namespace EasyStocks.View
{
    partial class SearchShareView
    {
        private void OnSearchItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SearchShareViewModel;
            viewModel?.SelectShare(
                e.AddedItems
                .OfType<ShareDailyInformation>()
                .FirstOrDefault());
        }
    }
}
