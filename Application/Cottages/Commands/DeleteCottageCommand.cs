using MediatR;

namespace MobileAppCottage.Application.Cottages.Commands.DeleteCottage
{
    public class DeleteCottageCommand : IRequest
    {
        public int Id { get; }

        public DeleteCottageCommand(int id)
        {
            Id = id;
        }
    }
}