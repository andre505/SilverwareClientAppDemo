using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Services
{

    public class Utilities : IUtilities
    {
        private readonly ILogger _logger;

        public Utilities(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<RestResponse> MakeHttpRequest(object request, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers)
        {

            try
            {
                RestClientOptions opts = new RestClientOptions();
                opts.RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                opts.BaseUrl = new Uri($"{baseAddress}{requestUri}");

                using (RestClient client = new RestClient(opts))
                {
                    var restRequest = new RestRequest();

                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Content-Type", "application/json");

                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            restRequest.AddHeader(header.Key, header.Value);
                        }
                    }

                    if (method == HttpMethod.Post)
                    {
                        string data = JsonConvert.SerializeObject(request);
                        _logger.LogInformation($"Making POST Request to {baseAddress}{requestUri} Request Content: {data}");
                        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        //response = await client.PostAsync(requestUri, content);
                        restRequest.Method = Method.Post;

                        restRequest.AddStringBody(data, "application/json");

                        RestResponse restResponse = await client.ExecuteAsync(restRequest);
                        var serializedResponse = restResponse.Content.ToString();
                        _logger.LogInformation($"Response to URL: {baseAddress}{requestUri} Content: {serializedResponse}");
                        return restResponse;
                    }
                    else if (method == HttpMethod.Get)
                    {
                        _logger.LogInformation($"Making GET Request to {baseAddress}{requestUri}");
                        RestResponse restResponse = await client.ExecuteAsync(restRequest);
                        var serializedResponse = restResponse.Content.ToString();
                        _logger.LogInformation($"Response to URL: {baseAddress}{requestUri} Content: {serializedResponse}");
                        return restResponse;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"MakeHttpRequest Exception {baseAddress}{requestUri} {ex.Message}");
                return null;
            }
        }

    }
}
