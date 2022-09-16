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
    public class GetPOSServiceByIdQuery : IRequest<Response<POSService>>
    {
        public int Id { get; set; }
        public class GetPOSServiceByIdQueryHandler : IRequestHandler<GetPOSServiceByIdQuery, Response<POSService>>
        {
            private readonly IPOSServiceRepositoryAsync _posServiceRepository;
            public GetPOSServiceByIdQueryHandler(IPOSServiceRepositoryAsync posServiceRepository)
            {
                _posServiceRepository = posServiceRepository;
            }
            public async Task<Response<POSService>> Handle(GetPOSServiceByIdQuery query, CancellationToken cancellationToken)
            {
                var posService = await _posServiceRepository.GetByIdAsync(query.Id);
                if (posService == null) throw new ApiException($"POS Service Not Found.");

                return new Response<POSService>(posService, $"POS Service retrieval successful");

            }
        }
    }
}
