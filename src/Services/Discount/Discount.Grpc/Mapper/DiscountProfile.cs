using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            // dead simple due to same name fields, reverse = bidirectional
            CreateMap<Coupon, CouponModel>().ReverseMap(); 
        }
    }
}
