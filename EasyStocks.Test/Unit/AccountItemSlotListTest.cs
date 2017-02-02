using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyStocks.Model;
using EasyStocks.ViewModel;
using NUnit.Framework;

namespace EasyStocks.Test.Unit
{
    [TestFixture]
    public class AccountItemSlotListTest
    {
        [Test]
        public void AddNewItemToList()
        {
            var first = new AccountItemSlot(
                new AccountItemId(),"FIRST",1);
            var third =
                new AccountItemSlot(
                new AccountItemId(),"THIRD",3);
            var second = 
                new ReadonlyAccountItem(
                    new AccountItemId(),
                    string.Empty,
                    "SECOND",
                    0.0f,
                    2.0f,
                    RateChange.Positive,false);

            var list = new AccountItemSlotList {third, first};
            list.AddNewAccountItem(second);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
        }

        [Test]
        public void MoveItemDownInList()
        {
            var third =
                new AccountItemSlot(
                new AccountItemId(), "THIRD", 3);
            var secondSlot =
               new AccountItemSlot(
               new AccountItemId(), "SECOND", 2);
            var first = new AccountItemSlot(
                new AccountItemId(), "FIRST", 1);
            var secondItem =
                new ReadonlyAccountItem(
                    secondSlot.Id,
                    string.Empty,
                    "SECOND",
                    0.0f,
                    -2.0f,
                    RateChange.Negative, false);
            var list = new AccountItemSlotList { third, secondSlot,first };
            list.ChangeAccountItem(secondItem);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("FIRST"));
            Assert.That(list[2].Symbol, Is.EqualTo("SECOND"));
        }

        [Test]
        public void MoveItemUpInList()
        {
            var third =
                new AccountItemSlot(
                new AccountItemId(), "THIRD", 3);
            var secondSlot =
               new AccountItemSlot(
               new AccountItemId(), "SECOND", -2);
            var first = new AccountItemSlot(
                new AccountItemId(), "FIRST", 1);
            var secondItem =
                new ReadonlyAccountItem(
                    secondSlot.Id,
                    string.Empty,
                    "SECOND",
                    0.0f,
                    2.0f,
                    RateChange.Negative, false);
            var list = new AccountItemSlotList { third, first, secondSlot };
            list.ChangeAccountItem(secondItem);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
        }

        [Test]
        public void ChangeItem_SamePosition()
        {
            var third =
                new AccountItemSlot(
                new AccountItemId(), "THIRD", 3);
            var secondSlot =
               new AccountItemSlot(
               new AccountItemId(), "SECOND", 2);
            var first = new AccountItemSlot(
                new AccountItemId(), "FIRST", 1);
            var secondItem =
                new ReadonlyAccountItem(
                    secondSlot.Id,
                    string.Empty,
                    "SECOND",
                    0.0f,
                    2.0f,
                    RateChange.Negative, false);
            var list = new AccountItemSlotList { third, secondSlot, first };
            list.ChangeAccountItem(secondItem);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
        }
    }
}
