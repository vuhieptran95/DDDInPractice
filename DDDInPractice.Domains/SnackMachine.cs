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
        protected decimal initialcustomerMoney;

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
            initialcustomerMoney = customerMoney;
            
            machineMoney += money;
        }

        public void HandleBuy(Slot slot)
        {
            var actualSlot = Slots.FirstOrDefault(s => s.Position == slot.Position);
            if (actualSlot == null)
            {
                throw new Exception("The item you're trying to buy doesn't exist");
            }
            
            if (actualSlot.Price > customerMoney)
            {
                throw new Exception("You don't have enough money to buy this item");
            }
            
            actualSlot.DecreaseQuantity();
            
            customerMoney -= actualSlot.Price;
        }

        public void ReturnMoney()
        {
            if (customerMoney == 0m)
            {
                throw new Exception("You have spent all your money, nothing left to return!");
            }
            
            if (initialcustomerMoney == customerMoney)
            {
                throw new Exception("You haven't bought anything yet! Please choose an item.");
            }
        }

        public Money BreakdownMoneyLargeToSmall(decimal amount)
        {
            (int Cent, int Penny, int Quarter, int OneDollar, int FiveDollar, int TwentyDollar) money = (0, 0, 0, 0, 0,
                0);
            var remain = Convert.ToInt32(amount * 100);

            money.TwentyDollar = remain / 2000;
            remain = remain % 2000;
            money.FiveDollar = remain / 500;
            remain = remain % 500;
            money.OneDollar = remain / 100;
            remain = remain % 100;
            money.Quarter = remain / 25;
            remain = remain % 25;
            money.Penny = remain / 10;
            remain = remain % 10;
            money.Cent = remain;
            
            return new Money(money.Cent, money.Penny, money.Quarter, money.OneDollar, money.FiveDollar, money.TwentyDollar);
        }

    }
}