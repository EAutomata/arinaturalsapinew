using AriNaturals.DTOs;
using AriNaturals.Entity;
using AutoMapper;

namespace AriNaturals.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
            CreateMap<ProductHighlight, ProductHighlightDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
        }
    }
}
