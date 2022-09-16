using Application.Features.Clients;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.POSServices
{
    public class GetAllPOSServicesQuery : IRequest<PagedResponse<IEnumerable<GetAllPOSServicesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllPOSServicesQueryHandler : IRequestHandler<GetAllPOSServicesQuery, PagedResponse<IEnumerable<GetAllPOSServicesViewModel>>>
    {
        private readonly IPOSServiceRepositoryAsync _posServiceRepository;
        private readonly IMapper _mapper;
        public GetAllPOSServicesQueryHandler(IPOSServiceRepositoryAsync posServiceRepository, IMapper mapper)
        {
            _posServiceRepository = posServiceRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllPOSServicesViewModel>>> Handle(GetAllPOSServicesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllPOSServicesParameter>(request);
            var posService = await _posServiceRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var posServiceViewModel = _mapper.Map<IEnumerable<GetAllPOSServicesViewModel>>(posService);
            return new PagedResponse<IEnumerable<GetAllPOSServicesViewModel>>(posServiceViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
