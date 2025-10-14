using AriNaturals.DTOs;
using AriNaturals.Entity;
using AutoMapper;

namespace AriNaturals.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {

            // Product <-> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Highlights, opt => opt.MapFrom(src => src.Highlights))
                .ReverseMap();

            // Variant <-> VariantDto
            CreateMap<ProductVariant, ProductVariantDto>()
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore()); // prevent circular reference

            // Image <-> ImageDto
            CreateMap<ProductImage, ProductImageDto>()
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // HighlightSection <-> HighlightSectionDto
            CreateMap<ProductHighlightSection, ProductHighlightSectionDto>()
                .ReverseMap()
                .ForMember(dest => dest.Highlight, opt => opt.Ignore()); // prevent circular reference

            // Highlight <-> HighlightDto
            CreateMap<ProductHighlight, ProductHighlightDto>()
                .ForMember(dest => dest.HighlightSections, opt => opt.MapFrom(src => src.HighlightSections))
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<ProductReview, ProductReviewDto>()
              .ReverseMap()
              .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
