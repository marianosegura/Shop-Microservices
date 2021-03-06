using Discount.Grpc.Entities;
using System.Threading.Tasks;


namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<bool> CreateDiscount(Coupon coupon);
        Task<Coupon> ReadDiscount(string productName);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
