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
    public class ProductController : ApiController
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("Products")]
        public IHttpActionResult Get()
        {
            return Json<List<Model.Product>>(_productService.GetProducts());
        }
    }
}
