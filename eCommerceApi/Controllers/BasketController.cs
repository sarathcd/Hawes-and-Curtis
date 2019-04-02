using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common.Service;
using Model = Common.Model;

namespace eCommerceApi.Controllers
{
    public class BasketController : ApiController
    {
        private IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        [Route("Basket")]
        public IHttpActionResult GetBasket(int basketId)
        {
            return Json<Model.Basket>(_basketService.GetBasket(basketId));
        }

        [HttpPost]
        [Route("Basket/Add/Product")]
        public IHttpActionResult AddProductToBasket([FromBody] int? basketId, int productId, int quantity)
        {
            return Json<Model.Basket>(_basketService.AddProductToBasket(basketId, productId, quantity));
        }

        [HttpPost]
        [Route("Basket/Delete/Product")]
        public IHttpActionResult RemoveProductFromBasket(int basketId, int productId)
        {
            return Json<Model.Basket>(_basketService.RemoveProductFromBasket(basketId, productId));
        }

        [HttpPost]
        [Route("Basket/Update/Product")]
        public IHttpActionResult UpdateProductQuantityInBasket(int basketId, int productId, int quantity)
        {
            return Json<Model.Basket>(_basketService.UpdateProductQuantityInBasket(basketId, productId, quantity));
        }

        [HttpPost]
        [Route("Basket/Delete")]
        public IHttpActionResult DeleteBasket(int basketId)
        {
            _basketService.DeleteBasket(basketId);
            return Ok();
        }
    }
}
