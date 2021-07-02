using System;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Tests.Repositories
{
    public class FakeDiscountVoucherRepository : IDiscountVoucherRepository
    {
        public static readonly string ValidCode = "12343567";
        public static readonly string ExpiredCode = "7654321";

        public DiscountVoucher Get(string code)
        {
            if(code == ValidCode)
                return new DiscountVoucher(10, DateTime.Now.AddDays(5));
            
            if(code == ExpiredCode)
                return new DiscountVoucher(10, DateTime.Now.AddDays(-5));
            
            return null;
        }
    }
}