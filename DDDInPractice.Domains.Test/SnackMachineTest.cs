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
            _sampleSlot1 = new Slot(1,"1", 10, 1.02m, 1);
            _sampleSlot2 = new Slot(2,"2", 15, 1.05m, 2);
            _sampleSlot3 = new Slot(3,"3", 20, 2.02m, 3);
            _zeroQuantitySlot = new Slot(2, "1", 0, 1, 0);
            _machineMoney = new Money(500, 100, 100, 100, 100, 100 );
            _customerMoney = new Money(0,5,2,1,1,0);
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
                slots = new List<Slot>(){_sampleSlot1, _sampleSlot1, _sampleSlot1};
                
                Action action  = () => AddSlots(_sampleSlot1);

                action.Should().Throw<Exception>("Cannot add more slot. Machine can only have maximum 3 slots");
            }
        }
        
        public class HandleBuyTest: SnackMachineTest
        {
            private decimal initialMoney;
            private decimal initialCustomerMoney;
            private int quantity;
            public HandleBuyTest()
            {
                LoadMoney(_machineMoney);
                initialMoney = _machineMoney.TotalInDollars();
                
                TakeMoney(_customerMoney);
                initialCustomerMoney = customerMoney;
                
                slots = new List<Slot>(){_sampleSlot1, _sampleSlot2, _sampleSlot3};
                quantity = _sampleSlot1.ProductCount;
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
            
        }
    }
}