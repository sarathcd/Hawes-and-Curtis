using System.Linq;

using Common.Enum;
using Common.Exception;
using Common.Service;
using Dal;
using Model = Common.Model;

namespace eCommerceService
{
    public class BasketService: IBasketService
    {
        private WarehouseRepostory _repository;
        public BasketService(WarehouseRepostory repostory)
        {
            _repository = repostory;
        }

        public Model.Basket AddProductToBasket(int? basketId, int productId, int quantity)
        {
            Basket basket;
            if (!basketId.HasValue || basketId.Value == 0)
            {
                basket = new Basket();
                _repository.AddBasket(basket);
            }
            else
                basket = _repository.Baskets.FirstOrDefault(b => b.BasketId == basketId.Value);

            if (basket == null)
                throw new ValidationException("Basket does not exist.");

            if(!_repository.Products.Any(p => p.ProductId == productId))
                throw new ValidationException("Product does not exist.");

            var availableQuantity = _repository.ProductQuantities.Where(pq => pq.ProductId == productId).GetAvailableProductQuantity();
            if (quantity > availableQuantity)
                throw new ValidationException("Requested quantity does not exist.");

            var transactionQuantity = new ProductTransactionQuantity
            {
                ProductId = productId,
                Quantity = quantity,
                TransactionTypeId = (int)TransactionType.Requested
            };
            _repository.AddProductTransactionQuantity(transactionQuantity);

            var basketProduct = new BasketProduct
            {
                BasketId = basket.BasketId,
                ProductTransactionQuantityId = transactionQuantity.ProductTransactionQuantityId
            };
            _repository.AddBasketProduct(basketProduct);

            return this.GetBasket(basket.BasketId);
        }

        public void DeleteBasket(int basketId)
        {
            var basket = _repository.Baskets.FirstOrDefault(b => b.BasketId == basketId);

            if (basket == null)
                throw new ValidationException("Basket does not exist.");

            var transactionQuantity = _repository.BasketProducts
                .Where(bp => bp.BasketId == basketId)
                .Join(_repository.ProductQuantities, bp => bp.ProductTransactionQuantityId, pq => pq.ProductTransactionQuantityId, (bp, pq) => pq)
                .ToList();

            foreach (var pq in transactionQuantity)
                _repository.AddProductTransactionQuantity(new ProductTransactionQuantity
                {
                    ProductId = pq.ProductId,
                    Quantity = pq.Quantity,
                    TransactionTypeId = (int)TransactionType.CancelledRequest
                });

            var basketProducts = _repository.BasketProducts.Where(bp => bp.BasketId == basketId).ToList();
            foreach (var bp in basketProducts)
                _repository.DeleteBasketProduct(bp);

            _repository.DeleteBasket(basket);
        }

        public Model.Basket GetBasket(int basketId)
        {
            var basket = new Model.Basket();
            basket.BasketId = _repository.Baskets.First(b => b.BasketId == basketId).BasketId;

            var productQuery = from bp in _repository.BasketProducts
                               join pq in _repository.ProductQuantities on bp.ProductTransactionQuantityId equals pq.ProductTransactionQuantityId
                               join p in _repository.Products on pq.ProductId equals p.ProductId
                               where pq.TransactionTypeId == (int) TransactionType.Requested
                               select new Model.Product
                               {
                                   ProductId = p.ProductId,
                                   Name = p.Name,
                                   Quantity = pq.Quantity
                               };

            basket.Products = productQuery.ToList();

            return basket;
        }

        public Model.Basket RemoveProductFromBasket(int basketId, int productId)
        {
            var basket = _repository.Baskets.FirstOrDefault(b => b.BasketId == basketId);

            if (basket == null)
                throw new ValidationException("Basket does not exist.");

            if (!_repository.Products.Any(p => p.ProductId == productId))
                throw new ValidationException("Product does not exist.");

            var transactionQuantity = _repository.BasketProducts
                .Where(bp => bp.BasketId == basketId)
                .Join(_repository.ProductQuantities.Where(pq => pq.ProductId == productId), bp => bp.ProductTransactionQuantityId, pq => pq.ProductTransactionQuantityId, (bp, pq) => pq)
                .FirstOrDefault();

            if (transactionQuantity != null)
            {
                _repository.ProductQuantities.Add(new ProductTransactionQuantity
                {
                    ProductId = transactionQuantity.ProductId,
                    Quantity = transactionQuantity.Quantity,
                    TransactionTypeId = (int)TransactionType.CancelledRequest
                });

                var basketProduct = _repository.BasketProducts.FirstOrDefault(bp => bp.ProductTransactionQuantityId == transactionQuantity.ProductTransactionQuantityId);

                if (basketProduct != null)
                    _repository.DeleteBasketProduct(basketProduct);
            }

            return this.GetBasket(basket.BasketId);
        }

        public Model.Basket UpdateProductQuantityInBasket(int basketId, int productId, int quantity)
        {
            var basket = _repository.Baskets.FirstOrDefault(b => b.BasketId == basketId);

            if (basket == null)
                throw new ValidationException("Basket does not exist.");

            if (!_repository.Products.Any(p => p.ProductId == productId))
                throw new ValidationException("Product does not exist.");

            var availableQuantity = _repository.ProductQuantities.Where(pq => pq.ProductId == productId).GetAvailableProductQuantity();

            var transactionQuantity = (from bp in _repository.BasketProducts.Where(x => x.BasketId == basketId)
                     join pq in _repository.ProductQuantities.Where(x => x.ProductId == productId) on bp.ProductTransactionQuantityId equals pq.ProductTransactionQuantityId
                     select pq).FirstOrDefault();

            if (transactionQuantity != null)
            {
                if (quantity > (availableQuantity + transactionQuantity.Quantity))
                    throw new ValidationException("Requested quantity does not exist.");

                _repository.ProductQuantities.Add(new ProductTransactionQuantity
                {
                    ProductId = transactionQuantity.ProductId,
                    Quantity = transactionQuantity.Quantity,
                    TransactionTypeId = (int)TransactionType.CancelledRequest
                });

                var newTransactionQuantity = new ProductTransactionQuantity
                {
                    ProductId = transactionQuantity.ProductId,
                    Quantity = quantity,
                    TransactionTypeId = (int)TransactionType.Requested
                };
                _repository.ProductQuantities.Add(newTransactionQuantity);

                var basketProduct = _repository.BasketProducts.FirstOrDefault(bp => bp.ProductTransactionQuantityId == transactionQuantity.ProductTransactionQuantityId);

                basketProduct.ProductTransactionQuantityId = newTransactionQuantity.ProductTransactionQuantityId;

                _repository.UpdateBasketProduct(basketProduct);
            }
            else
                this.AddProductToBasket(basketId, productId, quantity);

            return this.GetBasket(basketId);
        }
    }
}
