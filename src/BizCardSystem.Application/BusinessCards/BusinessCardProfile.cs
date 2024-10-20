using AutoMapper;
using BizCardSystem.Application.BusinessCards.Dtos.Create;
using BizCardSystem.Application.BusinessCards.Dtos.Get;
using BizCardSystem.Application.BusinessCards.Dtos.Update;
using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Domain.BusinessCards.Filters;
using BizCardSystem.Domain.Enums;
using BizCardSystem.Domain.FileHelper;
using BizCardSystem.Domain.Shared;

namespace BizCardSystem.Application.BusinessCards;

public class BusinessCardProfile : Profile
{
    public BusinessCardProfile()
    {
        CreateMap<BusinessCard, CreateBizRequest>().ReverseMap();
        CreateMap<BusinessCard, GetBizResponse>().ReverseMap();
        CreateMap<BusinessCard, UpdateBizRequest>().ReverseMap();



        CreateMap<BusinessCard, GetBizResponse>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.Country}, {src.Address.State}, {src.Address.City}, {src.Address.Street}"))
            .ForMember(dest => dest.DateofBirth, opt => opt.MapFrom(src => src.DateofBirth.ToString()))
            .ReverseMap();
        CreateMap<BusinessCard, FileParser>()
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
        .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.Country}, {src.Address.State}, {src.Address.City}, {src.Address.Street}"))
        .ForMember(dest => dest.DateofBirth, opt => opt.MapFrom(src => src.DateofBirth.ToString()))
        .ReverseMap()
        .ForMember(dest => dest.DateofBirth, opt => opt.MapFrom(src => DateTime.Parse(src.DateofBirth)))
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender)))
        .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
         ParseAddress(src.Address)));

        CreateMap<Address, AddressDto>()
            .ReverseMap();

        CreateMap<BusinessCardFilter, BusinessCardParameters>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ReverseMap();
    }
    private static Address ParseAddress(string addressString)
    {
        var parts = addressString?.Split(',') ?? new string[5];

        return new Address(
            Country: parts.Length > 0 ? parts[0].Trim() : null,
            State: parts.Length > 1 ? parts[1].Trim() : null,
            ZipCode: parts.Length > 2 ? parts[2].Trim() : null,
            City: parts.Length > 3 ? parts[3].Trim() : null,
            Street: parts.Length > 4 ? parts[4].Trim() : null
        );
    }
}
