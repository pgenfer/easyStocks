using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EasyStocks.Model;

namespace EasyStocks.ViewModel
{
    public enum SortMode
    {
        ByDailyChangeDescending
    }


    public class AccountItemSlotList : BindableCollection<AccountItemSlot>
    {
        // private SortMode _sortMode = SortMode.ByDailyChangeDescending;

        public void AddNewAccountItem(ReadonlyAccountItem newAccountItem)
        {
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
            if(!added)
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
        { }

        public void ChangeAccountItem(ReadonlyAccountItem accountItem)
        {
            // old position in list
            var oldPos = 0;
            var newPos = Count - 1;
            for (var i = 0; i < Count; i++)
            {
                if (Equals(accountItem.Id, this[i].Id))
                {
                    oldPos = i;
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
                this[oldPos].Set(accountItem);
                return;
            }
            // 1. item is moved up in list
            if (oldPos < newPos)
            {
                for (var i = oldPos; i < newPos; i++)
                    this[i].Set(this[i + 1].Copy());
                this[newPos].Set(accountItem);
                return;
            }
            // 2. item is moved down
            if (oldPos > newPos)
            {
                var next = this[newPos].Copy();
                this[newPos].Set(accountItem);
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
