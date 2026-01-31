using MediatR;
using MobileAppCottage.Domain.Interfaces;

namespace MobileAppCottage.Application.Cottages.Commands.DeleteCottage
{
    public class DeleteCottageCommandHandler : IRequestHandler<DeleteCottageCommand>
    {
        private readonly ICottageRepository _cottageRepository;

        public DeleteCottageCommandHandler(ICottageRepository cottageRepository)
        {
            _cottageRepository = cottageRepository;
        }

        public async Task Handle(DeleteCottageCommand request, CancellationToken cancellationToken)
        {
            await _cottageRepository.Delete(request.Id);
            // Brak return - to naprawia błąd CS0738
        }
    }
}