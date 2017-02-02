namespace EasyStocks.Model
{
    /// <summary>
    /// an id that every account item
    /// gets during generation. The ids are not
    /// serialized and will be generated every time
    /// a new account item is created.
    /// </summary>
    public class AccountItemId
    {
        private readonly int _id;
        private static int _InstanceCounter = 1;

        public AccountItemId()
        {
            _id = _InstanceCounter++;
        }

        public override bool Equals(object obj) => _id == ((AccountItemId)obj)?._id;
        public override int GetHashCode() => _id.GetHashCode();
    }
}