using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public class PortfolioSearchViewModel : Screen
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

        public PortfolioSearchViewModel(
            PortfolioViewModel portfolioViewModel,
            SearchShareViewModel searchViewModel)
        {
            Portfolio = portfolioViewModel;
            Search = searchViewModel;

            Items = new BindableCollection<string> {"hello 1", "hello 2"};
        }

        public BindableCollection<string> Items { get; set; }
    }
}
