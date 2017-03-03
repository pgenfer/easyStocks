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
                    RateChange.Positive,
                    false,
                    DateTime.Now);

            var list = new AccountItemSlotList {third, first};
            list.AddNewAccountItem(second);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
        }

        [Test]
        public void AddItemWithSameSymbolToList()
        {
            // Arrange: two items with the same symbol
            var first = new ReadonlyAccountItem(
                new AccountItemId(), "FIRST", "FIRST", 1.0f, 1.0f, RateChange.Positive, false, DateTime.Now);
            var nextFirst = new ReadonlyAccountItem(
                new AccountItemId(), "FIRST", "FIRST", 1.0f, 1.0f, RateChange.Positive, false, DateTime.Now);
            // Act: add both items to the list
            var list = new AccountItemSlotList();
            list.AddNewAccountItem(first);
            list.AddNewAccountItem(nextFirst);
            // Assert: both items should be grouped in one slot item
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].Ids.Count, Is.EqualTo(2));
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
                    secondSlot.Ids.Single(),
                    string.Empty,
                    "MINUS SECOND",
                    0.0f,
                    -2.0f,
                    RateChange.Negative,
                    false,
                    DateTime.Now);
            var list = new AccountItemSlotList { third, secondSlot,first };
            list.ChangeAccountItem(secondItem);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("FIRST"));
            Assert.That(list[2].Symbol, Is.EqualTo("MINUS SECOND"));
        }

        [Test]
        public void MoveItemDownInList2()
        {
            // BEFORE:  AFTER:
            // 4        4
            // 3        2
            // 2        1
            // 0        0
            // 
            // 3 changed to 1
            // this is a different case than the first MoveItemDownInList
            // because the element that changes is not at the end

            var fourth = new AccountItemSlot(
               new AccountItemId(), "FOURTH", 4);
            var third = new AccountItemSlot(
               new AccountItemId(), "THIRD", 3);
            var second = new AccountItemSlot(
               new AccountItemId(), "SECOND", 2);
            var zero = new AccountItemSlot(
                new AccountItemId(), "ZERO",0 );
            var thirdToFirst =
                new ReadonlyAccountItem(
                    third.Ids.Single(),
                    string.Empty,
                    "FIRST",
                    0.0f,
                    1.0f,
                    RateChange.Negative, 
                    false,
                    DateTime.Now);
            var list = new AccountItemSlotList { fourth,third,second,zero };
            list.ChangeAccountItem(thirdToFirst);

            Assert.That(list[0].Symbol, Is.EqualTo("FOURTH"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
            Assert.That(list[3].Symbol, Is.EqualTo("ZERO"));
        }

        [Test]
        public void MoveItemDownInList3()
        {
            var fourth =
                new AccountItemSlot(
                new AccountItemId(), "FOURTH", 4);
            var secondSlot =
               new AccountItemSlot(
               new AccountItemId(), "SECOND", 2);
            var zero = new AccountItemSlot(
                new AccountItemId(), "ZERO", 0);
            var fourthToFirst =
                new ReadonlyAccountItem(
                    fourth.Ids.Single(),
                    string.Empty,
                    "FIRST",
                    0.0f,
                    1.0f,
                    RateChange.Negative,
                    false,
                    DateTime.Now);
            var list = new AccountItemSlotList { fourth, secondSlot, zero };
            list.ChangeAccountItem(fourthToFirst);

            Assert.That(list[0].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[1].Symbol, Is.EqualTo("FIRST"));
            Assert.That(list[2].Symbol, Is.EqualTo("ZERO"));
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
                    secondSlot.Ids.Single(),
                    string.Empty,
                    "SECOND",
                    0.0f,
                    2.0f,
                    RateChange.Negative,
                    false,
                    DateTime.Now);
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
                    secondSlot.Ids.Single(),
                    string.Empty,
                    "SECOND",
                    0.0f,
                    2.0f,
                    RateChange.Negative,
                    false,
                    DateTime.Now);
            var list = new AccountItemSlotList { third, secondSlot, first };
            list.ChangeAccountItem(secondItem);

            Assert.That(list[0].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[1].Symbol, Is.EqualTo("SECOND"));
            Assert.That(list[2].Symbol, Is.EqualTo("FIRST"));
        }

        [Test]
        public void ChangeItem_DuplicateInList()
        {
            // Arrange: We have two items in the list, one slot holds two account ids
            var nextFirst = new ReadonlyAccountItem(
                new AccountItemId(), "FIRST", "FIRST", 1.0f, 4f, RateChange.Positive, false, DateTime.Now);
            var firstSlot = new AccountItemSlot(new AccountItemId(), "FIRST", 1.0f, 1.0f);
            firstSlot.AddAccountItem(nextFirst);
            var secondSlot =
               new AccountItemSlot(
               new AccountItemId(), "SECOND", 2);
            var third =
                new AccountItemSlot(
                new AccountItemId(), "THIRD", 3);
            var list = new AccountItemSlotList { third, secondSlot, firstSlot };
            // Act: the first item changes, should be at the top position now
            list.ChangeAccountItem(nextFirst);

            Assert.That(list[0].Symbol, Is.EqualTo("FIRST"));
            Assert.That(list[1].Symbol, Is.EqualTo("THIRD"));
            Assert.That(list[2].Symbol, Is.EqualTo("SECOND"));
        }
    }
}
