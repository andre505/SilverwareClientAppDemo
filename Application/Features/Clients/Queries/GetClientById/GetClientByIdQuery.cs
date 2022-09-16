using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class GetClientByIdQuery : IRequest<Response<Client>>
    {
        public int Id { get; set; }
        public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Response<Client>>
        {
            private readonly IClientRepositoryAsync _clientRepository;
            public GetClientByIdQueryHandler(IClientRepositoryAsync clientRepository)
            {
                _clientRepository = clientRepository;
            }
            public async Task<Response<Client>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
            {
                var client = await _clientRepository.GetByIdAsync(query.Id);
                if (client == null) throw new ApiException($"Client Not Found.");

                return new Response<Client>(client, $"Client retrieval successful");

            }
        }
    }
}
