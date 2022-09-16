using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class DeleteClientByIdCommand : IRequest<Response<Client>>
    {
        public int Id { get; set; }
        public class DeleteClientByIdCommandHandler : IRequestHandler<DeleteClientByIdCommand, Response<Client>>
        {
            private readonly IClientRepositoryAsync _clientRepository;
            public DeleteClientByIdCommandHandler(IClientRepositoryAsync clientRepository)
            {
                _clientRepository = clientRepository;
            }
            public async Task<Response<Client>> Handle(DeleteClientByIdCommand command, CancellationToken cancellationToken)
            {
                var client = await _clientRepository.GetByIdAsync(command.Id);
                if (client == null) throw new ApiException($"Client Not Found.");
                await _clientRepository.DeleteAsync(client);

                //return new Response<Product>(product.Id);

                return new Response<Client>(client, $"Client successfully deleted");

            }
        }
    }
}
