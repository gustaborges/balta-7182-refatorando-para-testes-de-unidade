using System;
using Flunt.Validations;

namespace Store.Domain.Entities
{
    public class OrderItem : Entity
    {
        public OrderItem(Product product, int quantity)
        {
            AddNotifications(new Contract<OrderItem>()
            .Requires()
            .IsNotNull(product, nameof(product), "Produto inválido")
            .IsGreaterThan(quantity, 0, nameof(quantity), "Quantidade deve ser maior que 0"));
            
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