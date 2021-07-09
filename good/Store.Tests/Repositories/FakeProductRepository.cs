using System;
using System.Collections.Generic;
using System.Linq;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Tests.Repositories
{
    public class FakeProductRepository : IProductRepository
    {
        private static IList<Product> _products = new List<Product>()
        {
            new Product("Produto 01", 10, true),
            new Product("Produto 02", 10, true),
            new Product("Produto 03", 10, true),
            new Product("Produto 04", 10, false),
            new Product("Produto 05", 10, false),
        };

        public IEnumerable<Product> Get(IEnumerable<Guid> ids)
        {
            return _products;
        }

        public static IList<Guid> GetGuids()
        {
            return _products.Select(x => x.Id).ToList();
        }
    }
}