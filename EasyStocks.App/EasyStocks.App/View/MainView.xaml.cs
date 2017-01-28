using System;
using System.Collections.Generic;
using System.Linq;
using EasyStocks.ViewModel;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace EasyStocks.View
{
    public partial class MainView
    {
        public const double SwipeOffset = 360;

        private void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (e.SwipeOffset >= SwipeOffset)
            {
                var accountItemViewModel = e.ItemData as AccountItemViewModel;
                if (accountItemViewModel != null)
                {
                    var mainViewModel = BindingContext as MainViewModel;
                    mainViewModel?.RemoveAccountItem(accountItemViewModel);
                }
            }
            PortfolioListView.ResetSwipe();
        }
    }
}
