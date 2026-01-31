using AutoMapper;
using MediatR;
using MobileAppCottage.Domain.Interfaces;

namespace MobileAppCottage.Application.Cottages.Commands.UpdateCottage
{
    public class UpdateCottageCommandHandler : IRequestHandler<UpdateCottageCommand>
    {
        private readonly ICottageRepository _cottageRepository;
        private readonly IMapper _mapper;

        public UpdateCottageCommandHandler(ICottageRepository cottageRepository, IMapper mapper)
        {
            _cottageRepository = cottageRepository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateCottageCommand request, CancellationToken cancellationToken)
        {
            var cottage = await _cottageRepository.GetById(request.Id);

            if (cottage != null)
            {
                _mapper.Map(request, cottage);
                await _cottageRepository.Update(request.Id, cottage);
            }
            // Brak return - to naprawia błąd CS0738
        }
    }
}