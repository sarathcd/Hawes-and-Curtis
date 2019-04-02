using System.Collections.Generic;
using System.Linq;

using Common.Enum;
using Common.Service;
using Dal;
using Model = Common.Model;


namespace eCommerceService
{
    public class ProductService : IProductService
    {
        private WarehouseRepostory _repository;
        public ProductService(WarehouseRepostory repostory)
        {
            _repository = repostory;
        }

        public List<Model.Product> GetProducts()
        {
            return _repository.Products
                .GroupJoin(_repository.ProductQuantities,
                    p => p.ProductId,
                    pq => pq.ProductId,
                    (p, pqs) => new Model.Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Quantity = pqs.GetAvailableProductQuantity()
                    })
                .ToList();
        }

        public Model.Product GetProduct(int productId)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
                return null;

            return new Model.Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Quantity = _repository.ProductQuantities.GetAvailableProductQuantity()
            };
        }
    }
}
