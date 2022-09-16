using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;

namespace Application.Features.Clients
{
    public class UpdateClientCommand : IRequest<Response<Client>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }

        public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Response<Client>>
        {
            private readonly IClientRepositoryAsync _clientRepository;

            public UpdateClientCommandHandler(IClientRepositoryAsync clientRepository)
            {
                _clientRepository = clientRepository;
            }
            public async Task<Response<Client>> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
            {
                var client = await _clientRepository.GetByIdAsync(command.Id);

                if (client == null)
                {
                    throw new ApiException($"Client Not Found.");
                }
                else
                {
                    client.Name = command.Name;
                    client.Description = command.Description;
                    client.EmailAddress = command.EmailAddress;
                    client.Address = command.Address;
                    await _clientRepository.UpdateAsync(client);
                    
                    return new Response<Client>(client, $"Client successfully created");
                }
            }
        }
    }
}
