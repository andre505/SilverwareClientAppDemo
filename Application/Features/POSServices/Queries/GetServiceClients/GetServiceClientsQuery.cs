using Application.Exceptions;
using Application.Features.Clients;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.POSServices
{
    public class GetServiceClientsQuery : IRequest<Response<ServiceClientsResponse>>
    {
        public int ServiceId { get; set; }
        public class GetServiceClientsQueryHandler : IRequestHandler<GetServiceClientsQuery, Response<ServiceClientsResponse>>
        {
            private readonly IPOSServiceRepositoryAsync posServiceRepositoryAsync;
            public GetServiceClientsQueryHandler(IPOSServiceRepositoryAsync posServiceRepositoryAsync)
            {
                this.posServiceRepositoryAsync = posServiceRepositoryAsync;
            }

            public async Task<Response<ServiceClientsResponse>> Handle(GetServiceClientsQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    ServiceClientsResponse serviceClientsResponse = new ServiceClientsResponse();

                    var posService = await posServiceRepositoryAsync.GetByIdAsync(query.ServiceId);

                    if (posService == null)
                    {
                        return new Response<ServiceClientsResponse>(new ServiceClientsResponse { }, message: $"POS Service not found.", isSuccessful: false);
                    }

                    var serviceClients = await posServiceRepositoryAsync.GetServiceClients(query.ServiceId);
                    if (serviceClients == null)
                    {
                        return new Response<ServiceClientsResponse>(new ServiceClientsResponse { }, message: $"No clients are currently subscrined to service {posService.ServiceName}", isSuccessful: false);
                    }
                  
                    return new Response<ServiceClientsResponse>(serviceClientsResponse, message: $"Successfully retrieved clients for the specified service.", isSuccessful: true);
                }
                catch (Exception ex)
                {
                    throw new ApiException($"Service not found. {ex.Message}");
                }
            }
        }
    }
}
