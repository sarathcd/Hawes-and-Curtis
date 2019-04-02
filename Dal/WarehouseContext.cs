using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
    public class ProductTransactionQuantity
    {
        public int ProductTransactionQuantityId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int TransactionTypeId { get; set; }
    }

    public class Basket
    {
        public int BasketId { get; set; }
    }

    public class BasketProduct
    {
        public int BasketProductId { get; set; }
        public int BasketId { get; set; }
        public int ProductTransactionQuantityId { get; set; }
    }

    public class WarehouseRepostory
    {
        public List<Product> Products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Pen" },
            new Product { ProductId = 2, Name = "Notebook" },
            new Product { ProductId = 3, Name = "Ruler" },
            new Product { ProductId = 4, Name = "Bag" },
            new Product { ProductId = 5, Name = "Pencil" }
        };
        public List<ProductTransactionQuantity> ProductQuantities = new List<ProductTransactionQuantity>
        {
            new ProductTransactionQuantity { ProductTransactionQuantityId = 1, ProductId = 1, TransactionTypeId = 1, Quantity = 5 },
            new ProductTransactionQuantity { ProductTransactionQuantityId = 2,  ProductId = 2, TransactionTypeId = 1, Quantity = 3 },
            new ProductTransactionQuantity { ProductTransactionQuantityId = 3,  ProductId = 3, TransactionTypeId = 1, Quantity = 6 },
            new ProductTransactionQuantity { ProductTransactionQuantityId = 4,  ProductId = 4, TransactionTypeId = 1, Quantity = 8 },
            new ProductTransactionQuantity { ProductTransactionQuantityId = 5,  ProductId = 5, TransactionTypeId = 1, Quantity = 10 }
        };
        public List<Basket> Baskets = new List<Basket>();
        public List<BasketProduct> BasketProducts = new List<BasketProduct>();

        public void AddProductTransactionQuantity(ProductTransactionQuantity transactionQuantity)
        {
            transactionQuantity.ProductTransactionQuantityId = this.ProductQuantities.Count() + 1;
            this.ProductQuantities.Add(transactionQuantity);
        }

        public void AddBasket(Basket basket)
        {
            basket.BasketId = this.Baskets.Count() + 1;
            this.Baskets.Add(basket);
        }

        public void DeleteBasket(Basket basket)
        {
            this.Baskets.Remove(basket);
        }

        public void AddBasketProduct(BasketProduct basketProduct)
        {
            basketProduct.BasketProductId = this.BasketProducts.Count() + 1;
            this.BasketProducts.Add(basketProduct);
        }


        public void UpdateBasketProduct(BasketProduct basketProduct)
        {
            var bp = this.BasketProducts.FirstOrDefault(x => x.BasketProductId == basketProduct.BasketProductId);
            if (bp == null)
                return;
            bp = basketProduct;
        }

        public void DeleteBasketProduct(BasketProduct basketProduct)
        {
            this.BasketProducts.Remove(basketProduct);
        }

        public BasketProduct GetBasketProduct(int basketProductID)
        {
            return this.BasketProducts.FirstOrDefault(bp => bp.BasketProductId == basketProductID);
        }

        public List<BasketProduct> GetBasketProducts(int basketId)
        {
            return this.BasketProducts.Where(bp => bp.BasketId == basketId).ToList();
        }
    }
}
