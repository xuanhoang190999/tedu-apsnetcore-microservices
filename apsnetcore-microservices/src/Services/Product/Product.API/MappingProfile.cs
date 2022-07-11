using AutoMapper;
using Infrastructure.Mappings;
using Product.API.Entities;
using Shared.DTOs;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, CatalogProduct>(); // .ReverseMap()
            CreateMap<UpdateProductDto, CatalogProduct>()
                .IgnoreAllNonExisting();

        }
    }
}
