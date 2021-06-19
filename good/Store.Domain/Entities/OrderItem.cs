using System;

namespace Store.Domain.Entities
{
    public class OrderItem : Entity
    {
        public OrderItem(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.Price = product != null ? product.Price : 0;
        }
        
        public int Quantity { get; }
        public decimal Price { get; }
        public Product Product { get; }

        public decimal Total()
        {
            return Price * Quantity;
        }
    }
}