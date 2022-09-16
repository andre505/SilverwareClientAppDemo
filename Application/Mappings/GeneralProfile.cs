

using Application.Features.Clients;
using Application.Features.POSServices;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.POSServices;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //Location
            //District
            CreateMap<Client, GetAllClientsViewModel>();
            CreateMap<CreateClientCommand, Client>().ReverseMap();
            CreateMap<GetAllClientsQuery, GetAllClientsParameter>();
            CreateMap<GetPartnerClientServicesQuery, GetPartnerClientServicesParameter>();

            CreateMap<POSService, GetAllPOSServicesViewModel>();
            CreateMap<CreatePOSServiceCommand, POSService>().ReverseMap();
            CreateMap<GetAllPOSServicesQuery, GetAllPOSServicesParameter>();
            CreateMap<GetServiceClientsQuery, GetServiceClientsParameter>();
        }
    }
}