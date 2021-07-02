using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Tests.Repositories
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        public static readonly string ValidDocument = "12345678910";

        public Customer Get(string document)
        {
            if(document == ValidDocument)
                return new Customer("Bruce Wayne", "bruce@wayne.com");

            return null;
        }
    }
}