using System.Collections.Generic;
using System.Linq;
using DDDInPractice.Domains;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class VendingMachineDto
    {
        public VendingMachineDto(VendingMachine machine)
        {
            SelectedSlots = machine.SelectedSlots.Select(s => s.Position).ToList();
            CurrentAmount = machine.CurrentAmountCustomerMoney;
            SpentAmount = machine.InitialCustomerMoney.Total() - machine.CurrentAmountCustomerMoney;
            IsInTransaction = machine.IsInTransaction;
            Slots = machine.Slots.Select(s => new Slot(s.Position, s.ProductName, s.Price)).ToList();
        }

        public List<string> SelectedSlots { get; set; }
        public int CurrentAmount { get; set; }
        public int SpentAmount { get; set; }
        public bool IsInTransaction { get; set; }
        public List<Slot> Slots { get; set; }

        public class Slot
        {
            public Slot(string position, string productName, int price)
            {
                Position = position;
                ProductName = productName;
                Price = price;
            }

            public string Position { get; set; }
            public string ProductName { get; set; }
            public int Price { get; set; }
        }
    }
}