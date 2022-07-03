using Discount.Grpc.Protos;
using System;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {  // wrapper class for the discount grpc service methods
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;


        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }


        public async Task<CouponModel> ReadDiscount(string productName)
        {
            var discountRequest = new ReadDiscountRequest { ProductName = productName };
            return await _discountProtoService.ReadDiscountAsync(discountRequest);  // calling visual studio generated method
        }
    }
}
