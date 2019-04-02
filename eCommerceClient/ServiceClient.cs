using RestSharp;
using System;
using System.Configuration;

namespace eCommerceClient
{
    public abstract class ServiceClient
    {
        readonly IRestClient _client;

        protected ServiceClient()
        {
            var baseUrl = ConfigurationManager.AppSettings["eCommerceApiUrl"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new Exception("Rest API end point is not configured.");

            _client = new RestClient();
            _client.BaseUrl = new Uri(baseUrl);
        }

        protected ServiceClient(string baseUrl)
        {
            _client = new RestClient();
            _client.BaseUrl = new Uri(baseUrl);
        }

        protected T Execute<T>(RestRequest request) where T : new()
        {
            var response = _client.Execute<T>(request);

            if (response.ErrorException != null)
                new ApplicationException(response.ErrorException.Message);

            return response.Data;
        }

        protected void Execute(RestRequest request)
        {
            var response = _client.Execute(request);

            if (response.ErrorException != null)
                new ApplicationException(response.ErrorException.Message);
        }
    }
}
