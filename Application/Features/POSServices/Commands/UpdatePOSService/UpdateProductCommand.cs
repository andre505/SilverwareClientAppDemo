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
    public class UpdatePOSServiceCommand : IRequest<Response<POSService>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public class UpdatePOSServiceCommandHandler : IRequestHandler<UpdatePOSServiceCommand, Response<POSService>>
        {
            private readonly IPOSServiceRepositoryAsync _posServiceRepository;
            public UpdatePOSServiceCommandHandler(IPOSServiceRepositoryAsync posServiceRepository)
            {
                _posServiceRepository = posServiceRepository;
            }
            public async Task<Response<POSService>> Handle(UpdatePOSServiceCommand command, CancellationToken cancellationToken)
            {
                var posService = await _posServiceRepository.GetByIdAsync(command.Id);

                if (posService == null)
                {
                    throw new ApiException($"POS Service Not Found.");
                }
                else
                {
                    posService.ServiceName = command.Name;
                    posService.ServiceDescription = command.Description;
                    await _posServiceRepository.UpdateAsync(posService);

                    return new Response<POSService>(posService, $"POS Service successfully created");
                }
            }
        }
    }
}
