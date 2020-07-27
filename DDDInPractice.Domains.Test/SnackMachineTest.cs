using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace DDDInPractice.Domains.Test
{
    public class SnackMachineTest : SnackMachine
    {
        private Slot _sampleSlot1;
        private Slot _sampleSlot2;
        private Slot _sampleSlot3;
        private Slot _nullSlot;
        private Slot _zeroQuantitySlot;
        private Money _machineMoney;
        private Money _customerMoney;

        public SnackMachineTest()
        {
            _sampleSlot1 = new Slot(1, "1", 10, 1m, 1);
            _sampleSlot2 = new Slot(2, "2", 15, 10.05m, 2);
            _sampleSlot3 = new Slot(3, "3", 20, 2.02m, 3);
            _zeroQuantitySlot = new Slot(4, "4", 0, 1.01m, 4);
            _machineMoney = new Money(500, 100, 100, 100, 100, 100);
            _customerMoney = new Money(0, 10, 4, 1, 1, 0);
        }

        public new class AddSlotsTest : SnackMachineTest
        {
            [Fact]
            public void Add1Slot_SlotsCountIncrease1()
            {
                AddSlots(_sampleSlot1);

                Slots.Count().Should().Be(1);
            }

            [Fact]
            public void AddNullSlot_ThrowException()
            {
                Action action = () => this.AddSlots(_nullSlot);

                action.Should().Throw<Exception>()
                    .WithMessage("Cannot add null slot");
            }

            [Fact]
            public void CanOnlyHaveMaximum3Slots()
            {
                slots = new List<Slot>() {_sampleSlot1, _sampleSlot1, _sampleSlot1};

                Action action = () => AddSlots(_sampleSlot1);

                action.Should().Throw<Exception>("Cannot add more slot. Machine can only have maximum 3 slots");
            }
        }

        public class HandleBuyTest : SnackMachineTest
        {
            private decimal initialMoney;
            private decimal initialCustomerMoney;
            private int quantity;

            public HandleBuyTest()
            {
                LoadMoney(_machineMoney);
                initialMoney = _machineMoney.TotalInDollars();

                slots = new List<Slot>() {_sampleSlot1, _sampleSlot2, _zeroQuantitySlot};
                quantity = _sampleSlot1.ProductCount;

                TakeMoney(_customerMoney);
                initialCustomerMoney = customerMoney;
            }

            [Fact]
            public void TakeCustomerMoney_MachineMoneyIncreaseSameAmount()
            {
                machineMoney.TotalInDollars().Should().Be(initialMoney + _customerMoney.TotalInDollars());
            }

            [Fact]
            public void SuccessPurchase_SlotQuantityDecrease1_CustomerMoneyDecreaseBySlotPrice()
            {
                HandleBuy(_sampleSlot1);

                _sampleSlot1.ProductCount.Should().Be(quantity - 1);

                customerMoney.Should().Be(initialCustomerMoney - _sampleSlot1.Price);
            }

            [Fact]
            public void FailedPurchase_NoItemLeftInSlot_ThrowException_CustomerMoneyStayTheSame()
            {
                Action action = () => HandleBuy(_zeroQuantitySlot);

                action.Should().Throw<Exception>().WithMessage("No items left in this slot");

                _zeroQuantitySlot.ProductCount.Should().Be(0);

                customerMoney.Should().Be(initialCustomerMoney);
            }

            [Fact]
            public void
                FailedPurchase_NotEnoughCustomerMoney_ThrowException_CustomerMoneyStayTheSame_SlotQuantityStayTheSame()
            {
                var intitialQuantity = _sampleSlot2.ProductCount;

                Action action = () => HandleBuy(_sampleSlot2);

                action.Should().Throw<Exception>().WithMessage("You don't have enough money to buy this item");

                _sampleSlot2.ProductCount.Should().Be(intitialQuantity);

                customerMoney.Should().Be(initialCustomerMoney);
            }

            [Fact]
            public void FailedPurchase_BuyANonExistSlot_ThrowException_CustomerMoneyStayTheSame()
            {
                Action action = () => HandleBuy(_sampleSlot3);

                action.Should().Throw<Exception>().WithMessage("The item you're trying to buy doesn't exist");

                customerMoney.Should().Be(initialCustomerMoney);
            }
        }

        public class ReturnMoneyTest : SnackMachineTest
        {
            private decimal initialMoney;
            private decimal initialCustomerMoney;

            public ReturnMoneyTest()
            {
                LoadMoney(_machineMoney);
                initialMoney = _machineMoney.TotalInDollars();

                slots = new List<Slot>() {_sampleSlot1, _sampleSlot2, _zeroQuantitySlot};
            }

            [Fact]
            public void CustomerBuyNothing_ThrowException_MachineMoneyStayTheSame()
            {
                var cusMoney = new Money(0, 0, 0, 0, 5, 0);

                TakeMoney(cusMoney);

                Action action = ReturnMoney;

                action.Should().Throw<Exception>()
                    .WithMessage("You haven't bought anything yet! Please choose an item.");

                machineMoney.TotalInDollars().Should().Be(initialMoney + customerMoney);
            }

            [Fact]
            public void NoCustomerMoneyLeft_ThrowException_MachineMoneyStayTheSame()
            {
                var cusMoney = new Money(0, 0, 0, 0, 0, 0);
                TakeMoney(cusMoney);

                Action action = ReturnMoney;

                action.Should().Throw<Exception>()
                    .WithMessage("You have spent all your money, nothing left to return!");

                machineMoney.TotalInDollars().Should().Be(initialMoney + customerMoney);
                
            }

            [Theory]
            [InlineData("0.77", 2, 0, 3, 0, 0, 0)]
            [InlineData("1", 0, 0, 0, 1, 0, 0)]
            [InlineData("1.02", 2, 0, 0, 1, 0, 0)]
            [InlineData("1.12", 2, 1, 0, 1, 0, 0)]
            [InlineData("1.25", 0, 0, 1, 1, 0, 0)]
            [InlineData("1.35", 0, 1, 1, 1, 0, 0)]
            [InlineData("5", 0, 0, 0, 0, 1, 0)]
            [InlineData("6.02", 2, 0, 0, 1, 1, 0)]
            [InlineData("6.12", 2, 1, 0, 1, 1, 0)]
            [InlineData("6.25", 0, 0, 1, 1, 1, 0)]
            [InlineData("6.35", 0, 1, 1, 1, 1, 0)]
            [InlineData("6.37", 2, 1, 1, 1, 1, 0)]
            public void BreakDownMoneyLargeToSmall(string amount, int cent, int penny, int quarter, int oneDollar, int fiveDollar,
                int twentyDollar)
            {
                var breakDownAmount = Math.Round(Convert.ToDecimal(amount), 2);

                var money = BreakdownMoneyLargeToSmall(breakDownAmount);
                
                var expected = new Money(cent, penny, quarter, oneDollar, fiveDollar, twentyDollar);

                money.FullyEquals(expected).Should().BeTrue();
            }

            [Theory] // Always assume machine money is sufficient to be returned
            [InlineData(10, 10, 10, 10, 10, 10, "0.77", 8, 10, 7, 10, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1", 10, 10, 10, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1.02", 8, 10, 10, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1.12", 8, 9, 10, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1.25", 10, 10, 9, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1.35", 10, 9, 9, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "1.37", 8, 9, 9, 9, 10, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "5", 10, 10, 10, 10, 9, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "6.02", 8, 10, 10, 9, 9, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "6.12", 8, 9, 10, 9, 9, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "6.25", 10, 10, 9, 9, 9, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "6.35", 10, 9, 9, 9, 9, 10)]
            [InlineData(10, 10, 10, 10, 10, 10, "6.37", 8, 9, 9, 9, 9, 10)]
            [InlineData(10, 10, 10, 5, 0, 0, "5", 10, 10, 10, 0, 0, 0)]
            [InlineData(10, 10, 10, 5, 0, 0, "6.02", 8, 10, 6, 0, 0, 0)]
            [InlineData(10, 10, 10, 5, 0, 0, "6.12", 8, 9, 6, 0, 0, 0)]
            [InlineData(10, 10, 10, 5, 0, 0, "6.25", 10, 10, 5, 0, 0, 0)]
            [InlineData(10, 10, 10, 5, 0, 0, "6.35", 10, 9, 5, 0, 0, 0)]
            [InlineData(10, 10, 10, 5, 0, 0, "6.37", 8, 9, 5, 0, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "15", 10, 10, 10, 5, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "16.02", 8, 10, 10, 4, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "16.12", 8, 9, 10, 4, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "16.25", 10, 10, 9, 4, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "16.35", 10, 9, 9, 4, 0, 0)]
            [InlineData(10, 10, 10, 10, 2, 0, "16.37", 8, 9, 9, 4, 0, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "15", 10, 10, 10, 10, 7, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "16.02", 8, 10, 10, 9, 7, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "16.12", 8, 9, 10, 9, 7, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "16.25", 10, 10, 9, 9, 7, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "16.35", 10, 9, 9, 9, 7, 0)]
            [InlineData(10, 10, 10, 10, 10, 0, "16.37", 8, 9, 9, 9, 7, 0)]
            public void
                SomeCustomerMoneyLeft_ReturnTheirMoneyFromLargeToSmall_CustomerMoneyBackToZero_MachineMoneySubtractReturnValue(
                    int cent, int penny, int quarter, int oneDollar, int fiveDollar, int twentyDollar,
                    string customerMoneyLeft, int afterCent, int afterPenny, int afterQuarter, int afterOneDollar,
                    int afterFiveDollar, int afterTwentyDollar)
            {
                customerMoney = Math.Round(Convert.ToDecimal(customerMoneyLeft), 2);

                machineMoney = new Money(cent, penny, quarter, oneDollar, fiveDollar, twentyDollar);

                var expected = new Money(afterCent, afterPenny, afterQuarter, afterOneDollar, afterFiveDollar,
                    afterTwentyDollar);

                ReturnMoney();

                machineMoney.FullyEquals(expected).Should().Be(true);
            }
        }
    }
}