using Application.DTOs;
using Application.Services.Interfaces;
using Infrastructure.Shared.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Shared.Services
{
    public interface IUtilities : IAutoDependencyService
    {
        Task<RestResponse> MakeHttpRequest(object request, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers);
    }


}
