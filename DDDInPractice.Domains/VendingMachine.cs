using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDDInPractice.Domains
{
    public partial class VendingMachine : AggregateRoot
    {
        protected ICollection<Slot> slots;
        protected ICollection<Slot> selectedSlots;
        protected Money machineMoney;
        protected Money currentMachineMoney;
        protected int currentAmountCustomerMoney;
        protected Money initialCustomerMoney;
        protected bool isInTransaction;

        public VendingMachine()
        {
            slots = new List<Slot>();
            selectedSlots = new List<Slot>();
            initialCustomerMoney = new Money();
            machineMoney = new Money();
            currentMachineMoney = new Money();
        }

        public int Id { get; protected set; }
        public Money MachineMoney => machineMoney;
        public Money CurrentMachineMoney => currentMachineMoney;
        public Money InitialCustomerMoney => initialCustomerMoney;
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
            currentMachineMoney = machineMoney.DeepClone();
        }

        public void TakeMoney(Money money)
        {
            currentAmountCustomerMoney += money.Total();
            initialCustomerMoney += money;

            currentMachineMoney += money;
        }

        public void StartTransaction()
        {
            if (isInTransaction)
            {
                throw new Exception("Someone is using the machine, please wait");
            }
            
            isInTransaction = true;

            // Clear customer money
            currentAmountCustomerMoney = 0;
            initialCustomerMoney = new Money();

            // clone current machine money
            currentMachineMoney = machineMoney.DeepClone();
            
            // Add Transaction started event
            AddEvents(new TransactionStarted(this.Id));
        }

        public void HandleSelectItems(string position)
        {
            var actualSlot = Slots.FirstOrDefault(s => s.Position == position);
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

        public void CalculateReturnMoney()
        {
            if (currentAmountCustomerMoney == 0)
            {
                throw new Exception("You have spent all your money, nothing left to return!");
            }

            currentMachineMoney -= currentAmountCustomerMoney;
        }

        public bool IsAbleToReturnMoney()
        {
            try
            {
                _ = currentMachineMoney - currentAmountCustomerMoney;

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

        public void MachineDropItems(out bool isSuccess)
        {
            Console.WriteLine("Dropping items...");

            isSuccess = true;
        }

        public void MachineDropChange(out bool isSuccess)
        {
            Console.WriteLine("Return customer money...");

            isSuccess = true;
        }

        public void FinalizeTransaction()
        {
            machineMoney = currentMachineMoney;
            currentAmountCustomerMoney = 0;
            initialCustomerMoney.Clear();
            selectedSlots.Clear();
        }
        
        public void Rollback()
        {
            selectedSlots.GroupBy(ss => ss.Position).ToList().ForEach(g =>
            {
                for (int i = 0; i < g.Count(); i++)
                {
                    g.First().IncreaseQuantity();
                }
            });

            currentAmountCustomerMoney = initialCustomerMoney.Total();
        }

        public void AddTransactionCommittedEvent()
        {
            var soldItems = selectedSlots.GroupBy(ss => ss.Position).Select(g =>
                new TransactionItem(g.First().ProductName, g.First().Price, g.Count())).ToList();
            var amountCaptured = initialCustomerMoney.Total() - currentAmountCustomerMoney;

            AddEvents(new TransactionCommitted(Id, amountCaptured, soldItems));
        }

        public void AddTransactionCancelledEvent(string reasonPhrase)
        {
            var cancelledItems = selectedSlots.GroupBy(ss => ss.Position).Select(g =>
                new TransactionItem(g.First().ProductName, g.First().Price, g.Count())).ToList();
            
            AddEvents(new TransactionCancelled(Id, initialCustomerMoney.Total(), cancelledItems, reasonPhrase));
        }
    }
}