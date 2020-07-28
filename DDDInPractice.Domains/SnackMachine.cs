using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDDInPractice.Domains
{
    public class SnackMachine
    {
        protected ICollection<Slot> slots;
        protected ICollection<Slot> selectedSlots;
        protected Money machineMoney;
        protected int currentAmountCustomerMoney;
        protected Money initialCustomerMoney;
        protected bool isInTransaction;

        public SnackMachine()
        {
            slots = new List<Slot>();
            selectedSlots = new List<Slot>();
            initialCustomerMoney = new Money();
            machineMoney = new Money();
        }

        public Money MachineMoney => machineMoney;
        public bool IsInTransaction => isInTransaction;
        public int CurrentAmountCustomerMoney => currentAmountCustomerMoney;
        public IEnumerable<Slot> Slots => slots;
        public IEnumerable<Slot> SelectedSlots => selectedSlots;

        public void AddSlots(Slot slot)
        {
            if (slot == null)
            {
                throw new Exception("Cannot add null slot");
            }

            if (Slots.Count() > 15)
            {
                throw new Exception("Cannot add more slot. Machine can only have maximum 16 slots");
            }

            slots.Add(slot);
        }

        public void LoadMoney(Money addedMoney)
        {
            machineMoney += addedMoney;
        }

        public void TakeMoney(Money money)
        {
            currentAmountCustomerMoney += money.Total();
            initialCustomerMoney += money;

            machineMoney += money;
        }

        public void StartTransaction()
        {
            isInTransaction = true;

            // Clear customer money
            currentAmountCustomerMoney = 0;
            initialCustomerMoney = new Money();

            // Raise Transaction started event
        }

        public void HandleSelectItems(Slot slot)
        {
            var actualSlot = Slots.FirstOrDefault(s => s.Position == slot.Position);
            if (actualSlot == null)
            {
                throw new Exception("The item you're trying to buy doesn't exist");
            }

            if (actualSlot.Price > currentAmountCustomerMoney)
            {
                throw new Exception("You don't have enough money left to buy this item");
            }

            actualSlot.DecreaseQuantity();

            currentAmountCustomerMoney -= actualSlot.Price;

            selectedSlots.Add(actualSlot);
        }

        public void RemoveLastSelectedSlot()
        {
            if (selectedSlots.Count < 1)
            {
                throw new Exception("You haven't chosen a slot yet");
            }

            var lastSlot = selectedSlots.Last();

            currentAmountCustomerMoney += lastSlot.Price;
            lastSlot.IncreaseQuantity();
            selectedSlots.Remove(lastSlot);
        }

        public void ReturnMoney()
        {
            if (currentAmountCustomerMoney == 0)
            {
                throw new Exception("You have spent all your money, nothing left to return!");
            }

            machineMoney -= currentAmountCustomerMoney;
        }

        public bool IsAbleToReturnMoney()
        {
            try
            {
                _ = machineMoney - currentAmountCustomerMoney;

                return true;
            }
            catch (Exception e)
            {
                if (e.Message == "Unable to do subtraction due to insufficient amount of notes")
                {
                    return false;
                }

                throw;
            }
        }

        public void CommitTransaction()
        {
            // Check ability to return money
            
            // Machine does its job

            // Return changes to customers
            
            // Reset customer money

            // Reset selectedSlots

            // Finish transaction

            // Add transaction committed event
        }

        public void CancelTransaction()
        {
            // Rollback selected slots and reset

            // Return customer's money

            // Finish transaction

            // Add transaction cancelled event
        }
    }
}