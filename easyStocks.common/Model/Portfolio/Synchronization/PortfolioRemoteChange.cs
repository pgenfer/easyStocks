using EasyStocks.Model.Account;

namespace EasyStocks.Model.Portfolio
{
    /// <summary>
    /// base class for storing changes to a repository that happened on a remote site
    /// and must be synchronized to the local repository.
    /// </summary>
    public abstract class PortfolioRemoteChange
    {
        /// <summary>
        /// derived classes will write the changed data to the local repository
        /// </summary>
        /// <param name="portfolio">portfolio where the changes should be applied to</param>
        public abstract void ApplyChangeToPortfolio(PortfolioRepository portfolio);
    }
}