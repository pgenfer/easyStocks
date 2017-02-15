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
    partial class MainView
    {
        private void OnPortfolioItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.Portfolio.SelectAccountItem(
                e.AddedItems
                .OfType<AccountItemSlot>()
                .FirstOrDefault()?.Id);
        }
    }
}
