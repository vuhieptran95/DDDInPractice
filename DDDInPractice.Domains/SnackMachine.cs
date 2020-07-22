using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDDInPractice.Domains
{
    public class SnackMachine
    {
        protected ICollection<Slot> slots;
        protected Money machineMoney;
        protected decimal customerMoney;

        public SnackMachine()
        {
            slots = new List<Slot>();
            machineMoney = new Money(0,0,0,0,0,0);
        }

        public Money MachineMoney => machineMoney;
        public decimal CustomerMoney => customerMoney;
        public IEnumerable<Slot> Slots => slots;

        public void AddSlots(Slot slot)
        {
            if (slot == null)
            {
                throw new Exception("Cannot add null slot");
            }
            if (Slots.Count() > 2)
            {
                throw new Exception("Cannot add more slot. Machine can only have maximum 3 slots");
            }
            
            slots.Add(slot);
        }

        public void LoadMoney(Money addedMoney)
        {
            machineMoney += addedMoney;
        }

        public void TakeMoney(Money money)
        {
            customerMoney = money.TotalInDollars();
            machineMoney += money;
        }

        public void HandleBuy(Slot slot)
        {
            var actualSlot = Slots.FirstOrDefault(s => s.Position == slot.Position);
            actualSlot.DecreaseQuantity();
            
            customerMoney -= actualSlot.Price;
        }

    }
}