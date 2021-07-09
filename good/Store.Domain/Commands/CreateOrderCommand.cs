using System;
using System.Collections.Generic;
using Flunt.Notifications;
using Flunt.Validations;
using Store.Domain.Commands.Interfaces;

namespace Store.Domain.Commands
{
    public class CreateOrderCommand : Notifiable<Notification>, ICommand
    {

        public CreateOrderCommand()
        {
            this.Items = new List<CreateOrderItemCommand>();
        }

        public CreateOrderCommand(string customer, string zipCode, string discountVoucher, IList<CreateOrderItemCommand> items)
        {
            this.Customer = customer;
            this.ZipCode = zipCode;
            this.DiscountVoucher = discountVoucher;
            this.Items = items;
        }

        public string Customer { get; set; }
        public string ZipCode { get; set; }
        public string DiscountVoucher { get; set; }
        public IList<CreateOrderItemCommand> Items { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<CreateOrderCommand>()
              .Requires()
              .AreEquals(Customer.Length, 11, nameof(Customer), "Cliente inválido")
              .AreEquals(ZipCode.Length, 8, nameof(ZipCode), "CEP inválido")
              .IsNotNull(Items, nameof(Items), "Nenhum item no pedido")
              .IsGreaterThan(Items.Count, 0, nameof(Items), "Nenhum item no pedido")
            );

            foreach (var item in Items)
            {
                item.Validate();
                AddNotifications(item);
            }                
        }
    }
}