using System;
using Flunt.Notifications;
using Flunt.Validations;
using Store.Domain.Commands.Interfaces;

namespace Store.Domain.Commands
{
    public class CreateOrderItemCommand : Notifiable<Notification>, ICommand
    {
        public CreateOrderItemCommand()
        {
        }
        
        public CreateOrderItemCommand(Guid product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }
        
        public int Quantity { get; set; }
        public Guid Product { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<CreateOrderItemCommand>()
                .Requires()
                .AreEquals(Product.ToString().Length, 36, nameof(Product), "Produto inválido")
                .IsGreaterThan(Quantity, 0, nameof(Quantity), "Quantidade deve ser maior que 0")
            );
        }
    }

    
}