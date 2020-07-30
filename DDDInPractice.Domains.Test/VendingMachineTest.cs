using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace DDDInPractice.Domains.Test
{
    public class VendingMachineTest : VendingMachine
    {
        private Slot _sampleSlot1;
        private Slot _sampleSlot2;
        private Slot _sampleSlot3;
        private Slot _nullSlot;
        private Slot _zeroQuantitySlot;
        private Money _machineMoney;
        private Money _customerMoney;

        public VendingMachineTest()
        {
            _sampleSlot1 = new Slot(1, "1", 10, 5, "1");
            _sampleSlot2 = new Slot(2, "2", 15, 10, "2");
            _sampleSlot3 = new Slot(3, "3", 20, 15, "3");
            _zeroQuantitySlot = new Slot(4, "4", 0, 50, "4");
            _machineMoney = new Money(200, 100, 50, 20, 10, 10, 4);
            _customerMoney = new Money(0, 0, 0, 1, 0, 0, 0);
        }

        public new class AddSlotsTest : VendingMachineTest
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
            public void CanOnlyHaveMaximum16Slots()
            {
                slots = new List<Slot>();

                for (int i = 0; i < 16; i++)
                {
                    slots.Add(_sampleSlot1);
                }

                Action action = () => AddSlots(_sampleSlot1);

                action.Should().Throw<Exception>("Cannot add more slot. Machine can only have maximum 16 slots");
            }
        }

        public class StartTransactionTest : VendingMachineTest
        {
            [Fact]
            public void WhenStart_FlagIsSetToTrue_CustomerMoneyResets()
            {
                this.StartTransaction();

                isInTransaction.Should().Be(true);
                currentAmountCustomerMoney.Should().Be(0);
                initialCustomerMoney.Should().Be(new Money());
            }
        }

        public class HandleSelectItemsTest : VendingMachineTest
        {
            private int quantity;

            public HandleSelectItemsTest()
            {
                StartTransaction();

                LoadMoney(_machineMoney);

                slots = new List<Slot>() {_sampleSlot1, _sampleSlot2, _zeroQuantitySlot};
                quantity = _sampleSlot1.ProductCount;

                TakeMoney(_customerMoney);
            }

            [Fact]
            public void TakeCustomerMoney_MachineMoneyIncreaseSameAmount()
            {
                currentMachineMoney.Should().Be(machineMoney + _customerMoney);
            }

            [Fact]
            public void SelectASlot_SlotQuantityDecrease1_SelectedSlotIncrease1_CustomerMoneyDecreaseBySlotPrice()
            {
                var selectedSlotsCount = selectedSlots.Count;
                var currentCustMoney = currentAmountCustomerMoney;

                HandleSelectItems(_sampleSlot1);

                _sampleSlot1.ProductCount.Should().Be(quantity - 1);

                selectedSlots.Count.Should().Be(selectedSlotsCount + 1);

                selectedSlots.Last().Should().Be(_sampleSlot1);

                currentAmountCustomerMoney.Should().Be(currentCustMoney - _sampleSlot1.Price);
            }

            [Fact]
            public void SelectASlot_NoItemLeftInSlot_ThrowException()
            {
                var selectedSlotsCount = selectedSlots.Count;
                var currentCustMoney = currentAmountCustomerMoney;

                Action action = () => HandleSelectItems(_zeroQuantitySlot);

                action.Should().Throw<Exception>().WithMessage("No items left in this slot");

                selectedSlots.Count.Should().Be(selectedSlotsCount);
                currentAmountCustomerMoney.Should().Be(currentCustMoney);
            }

            [Fact]
            public void
                SelectASlot_NotEnoughCustomerMoney_ThrowException()
            {
                var intitialQuantity = _sampleSlot2.ProductCount;
                var selectedSlotsCount = selectedSlots.Count;

                currentAmountCustomerMoney = 5;
                var currentCustMoney = currentAmountCustomerMoney;

                Action action = () => HandleSelectItems(_sampleSlot2);

                action.Should().Throw<Exception>().WithMessage("You don't have enough money left to buy this item");

                _sampleSlot2.ProductCount.Should().Be(intitialQuantity);
                selectedSlots.Count.Should().Be(selectedSlotsCount);
                currentAmountCustomerMoney.Should().Be(currentCustMoney);
            }

            [Fact]
            public void SelectANonExistSlot_ThrowException()
            {
                var selectedSlotsCount = selectedSlots.Count;

                Action action = () => HandleSelectItems(_sampleSlot3);

                action.Should().Throw<Exception>().WithMessage("The item you're trying to buy doesn't exist");
                selectedSlots.Count.Should().Be(selectedSlotsCount);
            }
        }

        public class ReturnMoneyTest : VendingMachineTest
        {
            public ReturnMoneyTest()
            {
                LoadMoney(_machineMoney);

                slots = new List<Slot>() {_sampleSlot1, _sampleSlot2, _zeroQuantitySlot};
            }

            [Fact]
            public void NoCustomerMoneyLeft_ThrowException()
            {
                currentAmountCustomerMoney = 0;

                Action action = ReturnMoney;

                action.Should().Throw<Exception>()
                    .WithMessage("You have spent all your money, nothing left to return!");

                currentMachineMoney.Total().Should().Be(machineMoney.Total() + currentAmountCustomerMoney);
            }

            [Fact]
            public void SomeCustomerMoneyLeft_ReturnMoneyUsingLargeToSmallNotes()
            {
                currentAmountCustomerMoney = 20;

                ReturnMoney();

                currentMachineMoney.Should().Be(new Money(200, 100, 49, 20, 10, 10, 4));
            }
        }
        
        public class RemoveLastSelectedSlotTest : VendingMachineTest
        {
            [Fact]
            public void NoSlotsInSelectedSlots_ThrowException()
            {
                currentAmountCustomerMoney = 100;
                selectedSlots = new List<Slot>();
                
                Action action = RemoveLastSelectedSlot;

                action.Should().Throw<Exception>().WithMessage("You haven't chosen a slot yet");
                currentAmountCustomerMoney.Should().Be(100);
            }

            [Fact]
            public void SelectedSlotsHaveItems_RemoveLastOne()
            {
                currentAmountCustomerMoney = 100;
                selectedSlots = new List<Slot>(){_sampleSlot1};
                var count = _sampleSlot1.ProductCount;
                
                RemoveLastSelectedSlot();

                selectedSlots.Count.Should().Be(0);
                _sampleSlot1.ProductCount.Should().Be(count + 1);
                currentAmountCustomerMoney.Should().Be(105);
            }
        }
    }
}