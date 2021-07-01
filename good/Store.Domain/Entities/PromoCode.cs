using System;

namespace Store.Domain.Entities
{
    public class PromoCode : Entity
    {
        public PromoCode(decimal amount, DateTime expireDate)
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