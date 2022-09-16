using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public partial class CreateClientCommand : IRequest<Response<Client>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string ClientType { get; set; }
        public int EmployeeUserId { get; set; }

    }
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Response<Client>>
    {
        private readonly IClientRepositoryAsync _clientRepository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public CreateClientCommandHandler(IClientRepositoryAsync clientRepository, IMapper mapper, IUserAccessor userAccessor)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
        }

        public async Task<Response<Client>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = _mapper.Map<Client>(request);
            await _clientRepository.AddAsync(client, _userAccessor.GetUserId());

            return new Response<Client>(client, $"Client successfully created");
        }
    }
}
