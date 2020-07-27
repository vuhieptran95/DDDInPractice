using System;

namespace DDDInPractice.Domains
{
    public class Slot
    {
        public Slot(int id, string productName, int productCount, decimal price, int position)
        {
            Id = id;
            ProductName = productName;
            ProductCount = productCount;
            Price = price;
            Position = position;
        }

        public int Id { get; protected set; }
        public string ProductName { get; protected set; }
        public int ProductCount { get; protected set; }
        public decimal Price { get; protected set; }
        public int Position { get; protected set; }
        
        public void DecreaseQuantity()
        {
            if (ProductCount < 1)
            {
                throw new Exception("No items left in this slot");
            }
            
            ProductCount -= 1;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                throw new Exception("Slot Product Name must have value");
            }
        }
    }
}