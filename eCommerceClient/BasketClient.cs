using System;
using Common.Service;
using RestSharp;
using Model = Common.Model;

namespace eCommerceClient
{
    public class BasketClient : ServiceClient, IBasketService
    {
        public BasketClient() { }
        public BasketClient(string baseUrl) : base(baseUrl) { }
        public Model.Basket AddProductToBasket(int? basketId, int productId, int quantity)
        {
            var request = new RestRequest("Basket/Add/Product", Method.POST);
            request.AddParameter("basketId", basketId, ParameterType.QueryString);
            request.AddParameter("productId", productId, ParameterType.QueryString);
            request.AddParameter("quantity", quantity, ParameterType.QueryString);
            return Execute<Model.Basket>(request);
        }

        public void DeleteBasket(int basketId)
        {
            var request = new RestRequest("Basket/Delete", Method.POST);
            request.AddParameter("basketId", basketId, ParameterType.QueryString);
            Execute(request);
        }

        public Model.Basket GetBasket(int basketId)
        {
            var request = new RestRequest("Basket", Method.GET);
            request.AddParameter("basketId", basketId);
            return Execute<Model.Basket>(request);
        }

        public Model.Basket RemoveProductFromBasket(int basketId, int productId)
        {
            var request = new RestRequest("Basket/Delete/Product", Method.POST);
            request.AddParameter("basketId", basketId, ParameterType.QueryString);
            request.AddParameter("productId", productId, ParameterType.QueryString);
            return Execute<Model.Basket>(request);
        }

        public Model.Basket UpdateProductQuantityInBasket(int basketId, int productId, int quantity)
        {
            var request = new RestRequest("Basket/Update/Product", Method.POST);
            request.AddParameter("basketId", basketId, ParameterType.QueryString);
            request.AddParameter("productId", productId, ParameterType.QueryString);
            request.AddParameter("quantity", quantity, ParameterType.QueryString);
            return Execute<Model.Basket>(request);
        }
    }
}
