using AutoMapper;
using MobileAppCottage.Application.Cottages.Commands.CreateCottage;
using MobileAppCottage.Application.Cottages.Commands.UpdateCottage;
using MobileAppCottage.Application.DTOs;
using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage.Application.Mappings
{
    public class CottageMappingProfile : Profile
    {
        public CottageMappingProfile()
        {
            // GET: Encja -> DTO
            CreateMap<Cottage, CottageDto>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.ContactDetails.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.ContactDetails.Street))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.ContactDetails.PostalCode));

            // ZAPIS/EDYCJA: Komendy -> CottageDetails (automatyczne dopasowanie pól)
            CreateMap<CreateCottageCommand, CottageDetails>();
            CreateMap<UpdateCottageCommand, CottageDetails>();

            // ZAPIS/EDYCJA: Komendy -> Cottage (główny obiekt)
            CreateMap<CreateCottageCommand, Cottage>()
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s));

            CreateMap<UpdateCottageCommand, Cottage>()
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s));
        }
    }
}