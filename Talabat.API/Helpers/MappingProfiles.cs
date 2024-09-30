using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.Order_Aggregation;
using OrderAdress = Talabat.Core.Entites.Order_Aggregation.Adress;
using IdentityAdress = Talabat.Core.Entites.Identity.Adress;

namespace Talabat.API.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<IdentityAdress, AdressDto>().ReverseMap();
            CreateMap<AdressDto, OrderAdress>();

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BaskeItemDto, BaskeItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

		}
	}
}
