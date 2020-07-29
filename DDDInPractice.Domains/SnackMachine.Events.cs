using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDDInPractice.Domains
{
    public partial class VendingMachine
    {
        public class TransactionStarted : DomainEvent
        {
            public TransactionStarted(int machineId)
            {
                MachineId = machineId;
            }
            public int MachineId { get; }
        }
        
        public class TransactionCancelled : DomainEvent
        {
            public TransactionCancelled(int machineId, int amountCancelled, IEnumerable<TransactionItem> items, string reasonPhrase)
            {
                MachineId = machineId;
                AmountCancelled = amountCancelled;
                Items = items;
                ReasonPhrase = reasonPhrase;
            }

            public int MachineId { get; }
            public int AmountCancelled { get; }
            public IEnumerable<TransactionItem> Items { get; }
            public string ReasonPhrase { get; }
        }
        
        public class TransactionCommitted : DomainEvent
        {
            public TransactionCommitted(int machineId, int actualAmountCaptured, IEnumerable<TransactionItem> transactionItems)
            {
                MachineId = machineId;
                ActualAmountCaptured = actualAmountCaptured;
                TransactionItems = transactionItems;
            }

            public int MachineId { get; }
            public int ActualAmountCaptured { get; }
            public IEnumerable<TransactionItem> TransactionItems { get;}
        }
        
        public class TransactionItem
        {
            public TransactionItem(string name, int price, int amount)
            {
                Name = name;
                Price = price;
                Amount = amount;
            }

            public string Name { get; }
            public int Price { get; }
            public int Amount { get; }
        }
    }
}