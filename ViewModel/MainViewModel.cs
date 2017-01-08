using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class MainViewModel : PropertyChangedBase
    {
        private PortfolioViewModel _portfolio;
        
        public PortfolioViewModel Portfolio
        {
            get { return _portfolio; }
            private set
            {
                if (Equals(value, _portfolio)) return;
                _portfolio = value;
                NotifyOfPropertyChange();
            }
        }

        public SearchShareViewModel Search { get; private set; }

        public MainViewModel(Portfolio portfolioBusinessModel,SearchShareViewModel searchViewModel)
        {
            Portfolio = new PortfolioViewModel(portfolioBusinessModel);
            Search = searchViewModel;
        }
    }
}
