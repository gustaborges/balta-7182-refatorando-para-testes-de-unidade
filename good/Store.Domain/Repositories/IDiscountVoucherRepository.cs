using Store.Domain.Entities;

namespace Store.Domain.Repositories
{
    public interface IDiscountVoucherRepository
    {
        DiscountVoucher Get(string code);
    }
}