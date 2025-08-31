using AriNaturals.DTOs;
using AriNaturals.Entity;
using AutoMapper;

namespace AriNaturals.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>()
                        .ForMember(dest => dest.Customer, opt => opt.Ignore())
                        .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                        .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                        .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                        .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src =>
                            src.IsBillingSameAsShipping ? src.ShippingAddress : src.BillingAddress));
            CreateMap<AddressDto, Address>();
            CreateMap<PaymentDto, Payment>();
            CreateMap<OrderItemDto, OrderItem>()
                        .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                        .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                        .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                        .ForMember(dest => dest.VariantId, opt => opt.MapFrom(src => src.VariantId))
                        .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

            CreateMap<Order, OrderDto>()
                        .ForMember(dest => dest.OrderDate,
                                   opt => opt.MapFrom(src => src.CreatedAt))
                        .ForMember(dest => dest.CustomerId,
                                   opt => opt.MapFrom(src => src.Customer.CustomerId))
                        .ForMember(dest => dest.CustomerName,
                                   opt => opt.MapFrom(src => src.Customer.FullName))
                        .ForMember(dest => dest.ShippingAddress,
                                   opt => opt.MapFrom(src => src.ShippingAddress))
                        .ForMember(dest => dest.Payment,
                                   opt => opt.Ignore());

            CreateMap<Address, AddressDto>();
            CreateMap<Payment, PaymentDto>();
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
