using System;
using System.Collections.Generic;
using System.Linq;
using EasyStocks.ViewModel;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace EasyStocks.View
{
    public partial class MainView
    {
       private void OnPortfolioItemTapped(object sender, ItemTappedEventArgs e)
        {
            var accountItemSlot = e.ItemData as AccountItemSlot;
            if (accountItemSlot != null)
            {
                var mainViewModel = BindingContext as MainViewModel;
                mainViewModel?.PortfolioAndSearch.Portfolio.SelectAccountItem(accountItemSlot.Id);
            }
        }

        public const double SwipeOffset = 360;

        private void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (e.SwipeOffset >= SwipeOffset)
            {
                var accountItemSlot = e.ItemData as AccountItemSlot;
                if (accountItemSlot != null)
                {
                    var mainViewModel = BindingContext as MainViewModel;
                    mainViewModel?.RemoveAccountItem(accountItemSlot.Id);
                }
            }
            PortfolioListView.ResetSwipe();
        }
    }
}
