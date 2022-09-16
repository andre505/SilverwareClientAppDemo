using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Entities.POSServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.POSServices
{ 
    public class DeletePOSServiceByIdCommand : IRequest<Response<POSService>>
    {
        public int Id { get; set; }
        public class DeletePOSServiceByIdCommandHandler : IRequestHandler<DeletePOSServiceByIdCommand, Response<POSService>>
        {
            private readonly IPOSServiceRepositoryAsync _posServiceRepository;
            public DeletePOSServiceByIdCommandHandler(IPOSServiceRepositoryAsync posServiceRepository)
            {
                _posServiceRepository = posServiceRepository;
            }
            public async Task<Response<POSService>> Handle(DeletePOSServiceByIdCommand command, CancellationToken cancellationToken)
            {
                var posService = await _posServiceRepository.GetByIdAsync(command.Id);
                if (posService == null) throw new ApiException($"POS Service Not Found.");
                await _posServiceRepository.DeleteAsync(posService);

                return new Response<POSService>(posService, $"POS Service successfully deleted");

            }
        }
    }
}
