using AutoMapper;
using Entities.Models;
using Services.Models;

namespace FridgeAPI.Extensions
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Fridge, FridgeResponse>();
            CreateMap<FridgeRequest, Fridge>();

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();

            CreateMap<FridgeProduct, FridgeProductResponse>()
                .ForMember(fm => fm.ProductName,
                opt => opt.MapFrom(x => x.Product.Name));
            CreateMap<FridgeProductRequest, FridgeProduct>();

            CreateMap<FridgeModel, FridgeModelResponse>();
            CreateMap<FridgeModelRequest, FridgeModel>();
        }
    }
}
