using System;

namespace Store.Domain.Entities
{
    public class DiscountVoucher : Entity
    {
        public DiscountVoucher(decimal amount, DateTime expireDate)
        {
            this.Amount = amount;
            this.ExpireDate = expireDate;
        }

        public DateTime ExpireDate { get; }
        public decimal Amount { get; }
        
        public decimal Value() => Valid() ? Amount : 0;
        public bool Valid() => ExpireDate > DateTime.Now;
    }
}