using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;

namespace FridgeAPI.Extensions
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Fridge, FridgeDto>();
            CreateMap<FridgeToCreateDto, Fridge>();
            CreateMap<FridgeToUpdateDto, Fridge>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductToCreateDto, Product>();
            CreateMap<ProductToUpdateDto, Product>();

            CreateMap<FridgeModel, FridgeProductDto>()
                .ForMember(fm => fm.ProductName,
                opt => opt.MapFrom(x => x.Name));
        }
    }
}
