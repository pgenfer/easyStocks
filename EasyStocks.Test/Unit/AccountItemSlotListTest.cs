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
                    "MINUS SECOND",
                    0.0f,
                    -2.0f,
                    RateChange.Negative, false);
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
                    third.Id,
                    string.Empty,
                    "FIRST",
                    0.0f,
                    1.0f,
                    RateChange.Negative, false);
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
                    fourth.Id,
                    string.Empty,
                    "FIRST",
                    0.0f,
                    1.0f,
                    RateChange.Negative, false);
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
