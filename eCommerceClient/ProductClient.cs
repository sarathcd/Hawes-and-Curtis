using System.Collections.Generic;
using Common.Service;
using RestSharp;
using Model = Common.Model;

namespace eCommerceClient
{
    public class ProductClient : ServiceClient, IProductService
    {
        public ProductClient() { }
        public ProductClient(string baseUrl) : base(baseUrl) { }
        public List<Model.Product> GetProducts()
        {
            return Execute<List<Model.Product>>(new RestRequest("Products", Method.GET));
        }
    }
}
