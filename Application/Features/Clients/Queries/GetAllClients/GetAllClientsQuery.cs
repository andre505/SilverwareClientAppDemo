using Application.Features.Clients;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class GetAllClientsQuery : IRequest<PagedResponse<IEnumerable<GetAllClientsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PagedResponse<IEnumerable<GetAllClientsViewModel>>>
    {
        private readonly IClientRepositoryAsync _clientRepository;
        private readonly IMapper _mapper;
        public GetAllClientsQueryHandler(IClientRepositoryAsync clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllClientsViewModel>>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllClientsParameter>(request);
            var client = await _clientRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var clientViewModel = _mapper.Map<IEnumerable<GetAllClientsViewModel>>(client);
            return new PagedResponse<IEnumerable<GetAllClientsViewModel>>(clientViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
