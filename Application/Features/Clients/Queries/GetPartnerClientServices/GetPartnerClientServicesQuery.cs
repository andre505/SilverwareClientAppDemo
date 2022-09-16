using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class GetPartnerClientServicesQuery : IRequest<Response<ClientServicesResponse>>
    {
        public int ClientId { get; set; }
        public class GetPartnerClientServicesQueryHandler : IRequestHandler<GetPartnerClientServicesQuery, Response<ClientServicesResponse>>
        {
            private readonly IClientRepositoryAsync clientRepositoryAsync;
            public GetPartnerClientServicesQueryHandler(IClientRepositoryAsync clientRepositoryAsync)
            {
                this.clientRepositoryAsync = clientRepositoryAsync;
            }

            public async Task<Response<ClientServicesResponse>> Handle(GetPartnerClientServicesQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    ClientServicesResponse clientServicesResponse = new ClientServicesResponse();

                    var partner = await clientRepositoryAsync.GetByIdAsync(query.ClientId);
                    var clientServices = await clientRepositoryAsync.GetPartnerClientServices(query.ClientId);
                    if (clientServices == null)
                    {
                        return new Response<ClientServicesResponse>(new ClientServicesResponse { }, message: $"No services are currently offered by the specified partner", isSuccessful: false);
                    }
                  
                    return new Response<ClientServicesResponse>(clientServicesResponse, message: $"Successfully services for partner {partner.Name}", isSuccessful: true);
                }
                catch (Exception ex)
                {
                    throw new ApiException($"Services not found. {ex.Message}");
                }
            }
        }
    }
}
