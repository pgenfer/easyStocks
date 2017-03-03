using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    /// <summary>
    /// visual representation of the user's portfolio.
    /// Account items with the same symbol are grouped together 
    /// to the same slot item.
    /// </summary>
    public class AccountItemSlotList : BindableCollection<AccountItemSlot>
    {
        private AccountItemSlot FindItemBySymbol(string symbol) => Items.FirstOrDefault(x => x.Symbol == symbol);

        /// <summary>
        /// a new account item is added to the list.
        /// If there is already a slot item with the same symbol,
        /// the item will be added there, otherwise a new item is added.
        /// </summary>
        /// <param name="newAccountItem"></param>
        public void AddNewAccountItem(ReadonlyAccountItem newAccountItem)
        {
            var existingAccountSlot = FindItemBySymbol(newAccountItem.Symbol);
            if (existingAccountSlot != null)
            {
                existingAccountSlot.AddAccountItem(newAccountItem);
                // no need to sort the list because
                // an item with the same id is already in the list at the correct position
                return;
            }

            var added = false;
            for (var i = 0; i < Count; i++)
            {
                var item = this[i];
                // item is higher than current item => add it here
                if (newAccountItem.DailyChangeInPercent > item.DailyChangeInPercent)
                {
                    added = true;
                    var next = item.Copy();
                    item.Set(newAccountItem);
                    // now move all items one up
                    for (var j = i + 1; j < Count; j++)
                    {
                        var tmp = this[j].Copy();
                        this[j].Set(next);
                        next = tmp;
                    }
                    // the last item must be added at the end
                    Add(new AccountItemSlot(next));
                    break;
                }
            }
            // item is smaller than any other item, add it at the end
            if (!added)
                Add(new AccountItemSlot(newAccountItem));
        }

        public void ChangeAccountItems(IEnumerable<ReadonlyAccountItem> accountItems)
        {
            foreach (var item in accountItems)
                ChangeAccountItem(item);
        }

        public void AddAccountItems(IEnumerable<ReadonlyAccountItem> accountItems)
        {
            foreach (var item in accountItems)
                AddNewAccountItem(item);
        }

        public void RemoveAccountItem(AccountItemId accountItem)
        {
            // normally, single should be enough here instead of FirstOrDefault,
            // but be a bit defensive in case an item was already deleted and
            // the list did not reflect it
            var item = this.FirstOrDefault(x => x.HasId(accountItem));
            item?.RemoveId(accountItem);
            // check if we removed the last account item from this id
            if (item != null && item.IsEmpty)
                Remove(item);
        }

        public void ChangeAccountItem(ReadonlyAccountItem accountItem)
        {
            // old position in list
            var oldPos = 0;
            var newPos = Count; // by default, we assume the item would be added at the end of the list
            // store all ids that are managed by the slot
            var accountItemsForSymbol = new List<AccountItemId>();
            for (var i = 0; i < Count; i++)
            {
                // we either have the account item or a slot with the same symbol
                if (this[i].HasId(accountItem.Id))
                {
                    oldPos = i;
                    accountItemsForSymbol = new List<AccountItemId>(this[i].Ids);
                    break;
                }
            }
            // new position of item
            for (var i = 0; i < Count; i++)
            {
                if (accountItem.DailyChangeInPercent >= this[i].DailyChangeInPercent)
                {
                    newPos = i;
                    break;
                }
            }

            // three cases:

            // 1. item stays at same position, just update the item
            if (oldPos == newPos)
            {
                this[oldPos].Set(accountItem, accountItemsForSymbol);
                return;
            }
            // 1. item is moved up in list
            if (oldPos < newPos)
            {
                // because we remove the item from the list
                // the new pos will be -1
                // but only if the item would not go to the end
                for (var i = oldPos; i < (newPos < Count - 1 ? newPos : Count - 1); i++)
                    this[i].Set(this[i + 1].Copy());
                this[--newPos].Set(accountItem, accountItemsForSymbol);
                return;
            }
            // 2. item is moved down
            if (oldPos > newPos)
            {
                var next = this[newPos].Copy();
                this[newPos].Set(accountItem, accountItemsForSymbol);
                for (var i = newPos + 1; i <= oldPos; i++)
                {
                    var tmp = this[i].Copy();
                    this[i].Set(next);
                    next = tmp;
                }
                return;
            }
        }
    }
}