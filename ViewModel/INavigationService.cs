using System.Threading.Tasks;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// interface for navigating between view models
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// navigates to the given view model and provides the given parameter
        /// when constructing the view model.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task NavigateToViewModel<TViewModel, TParameter>(TParameter parameter);
        Task NavigateToViewModel<TViewModel, TParameterOne,TParameterTwo>(TParameterOne parameterOne,TParameterTwo parameterTwo);
    }
}