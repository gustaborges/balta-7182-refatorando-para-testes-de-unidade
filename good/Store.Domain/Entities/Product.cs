using Flunt.Validations;

namespace Store.Domain.Entities
{
    public class Product : Entity
    {
        public Product(string title, decimal price, bool active)
        {
            AddNotifications(new Contract<Product>()
            .IsNotEmpty(title, nameof(title), "Título inválido")
            .IsGreaterThan(0m, price, nameof(price), "Preço inválido")
            );

            Active = active;
            Price = price;
            Title = title;
        }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }

        public void ChangeInfo(string title, decimal price, bool active)
        {
            Active = active;
            Price = price;
            Title = title;
        }
    }
}