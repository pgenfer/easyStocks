namespace EasyStocks.Error
{
    /// <summary>
    /// id that is used to identify an error.
    /// Every id can only occur at a specific place in the software
    /// </summary>
    public enum ErrorId
    {
        CannotLoadPortfolioFromStorage,
        CannotSavePortfolioToStorage,
        CannotClearPortfolioInStorage,
        CouldNotLoadFromDefaultStorage,
        CouldNotSaveToDefaultStorage,
        CannotRetrieveDailyStockData,
        CannotLookupStocks
    }
}