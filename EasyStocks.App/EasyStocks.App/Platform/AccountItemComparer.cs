using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Platform
{
    public class AccountItemComparer : IComparer<AccountItemViewModel>
    {
        public int Compare(AccountItemViewModel x, AccountItemViewModel y)
        {
            return x.ChangePercent.CompareTo(y.ChangePercent);
        }
    }
}
