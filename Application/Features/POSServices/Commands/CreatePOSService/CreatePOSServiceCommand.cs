using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Entities.POSServices;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.POSServices
{
    public partial class CreatePOSServiceCommand : IRequest<Response<POSService>>
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
 

    }
    public class CreatePOSServiceCommandHandler : IRequestHandler<CreatePOSServiceCommand, Response<POSService>>
    {
        private readonly IPOSServiceRepositoryAsync _posServiceRepository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public CreatePOSServiceCommandHandler(IPOSServiceRepositoryAsync posServiceRepository, IMapper mapper, IUserAccessor userAccessor)
        {
            _posServiceRepository = posServiceRepository;
            _mapper = mapper;
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
        }

        public async Task<Response<POSService>> Handle(CreatePOSServiceCommand request, CancellationToken cancellationToken)
        {
            var client = _mapper.Map<POSService>(request);
            await _posServiceRepository.AddAsync(client, _userAccessor.GetUserId());

            // return new Response<int>(product.Id);
            return new Response<POSService>(client, $"New POS Service successfully created");
        }
    }
}
