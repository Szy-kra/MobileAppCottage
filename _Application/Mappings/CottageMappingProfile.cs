using AutoMapper;
using MobileAppCottage.Application.DTOs;
using MobileAppCottage.Domain.Entities;

namespace MobileAppCottage._Application.Mappings
{
    public class CottageMappingProfile : Profile
    {
        public CottageMappingProfile()
        {
            // 1. Mapowanie Rezerwacji
            // Ponieważ DTO ma pola 'From' i 'To', a encja 'StartDate' i 'EndDate',
            // musimy wskazać AutoMapperowi co ma skąd brać.
            CreateMap<CottageReservation, ReservationDto>()
                .ForMember(m => m.From, c => c.MapFrom(s => s.StartDate))
                .ForMember(m => m.To, c => c.MapFrom(s => s.EndDate))
                .ForMember(m => m.IsAvailable, c => c.MapFrom(s => false));

            // 2. Mapowanie Domku (Cottage -> CottageDto)
            CreateMap<Cottage, CottageDto>()
                // Pola Name, Id, About, EncodedName mapują się automatycznie (takie same nazwy)

                // Mapowanie danych z powiązanej encji ContactDetails (Flattening)
                .ForMember(m => m.Description, c => c.MapFrom(s => s.ContactDetails.Description))
                .ForMember(m => m.Price, c => c.MapFrom(s => s.ContactDetails.Price))
                .ForMember(m => m.MaxPersons, c => c.MapFrom(s => s.ContactDetails.MaxPersons))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.ContactDetails.Street))
                .ForMember(m => m.City, c => c.MapFrom(s => s.ContactDetails.City))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.ContactDetails.PostalCode))

                // Mapowanie listy zdjęć: zamieniamy listę obiektów Image na listę stringów (URL)
                .ForMember(m => m.ImageUrls, c => c.MapFrom(s => s.Images.Select(img => img.Url).ToList()))

                // Mapowanie listy rezerwacji: używa definicji stworzonej powyżej (CreateMap<CottageReservation...>)
                .ForMember(m => m.BookedDates, c => c.MapFrom(s => s.Reservations));
        }
    }
}