using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// interface for navigating between view models.
    /// The navigation service is explicit, so that all
    /// possible navigations within the application are implemented,
    /// in that way no generic interface is needed
    /// </summary>
    public interface INavigationService
    {
        void NavigateToCreateAccountItem(ShareDailyInformation stockInformation);
        void NavigateToEditAccountItem(AccountItemId accountItemId);
        void NavigateToPortfolio();
        void NavigateToSearchView();
    }
}