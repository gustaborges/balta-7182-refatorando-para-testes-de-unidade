namespace Store.Domain.Entities
{
    public class Customer : Entity
    {
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }   
        public string Email { get; }
    }
}