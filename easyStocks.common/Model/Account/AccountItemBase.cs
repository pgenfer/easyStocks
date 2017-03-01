namespace EasyStocks.Model
{
    /// <summary>
    /// base class for account items
    /// </summary>
    public abstract class AccountItemBase
    {
        public AccountItemId Id { get; }
        public string ShareName { get; }
        public string Symbol { get; }

        protected AccountItemBase(
            AccountItemId id, 
            string shareName, 
            string symbol)
        {
            Id = id;
            ShareName = shareName;
            Symbol = symbol;
        }
    }
}