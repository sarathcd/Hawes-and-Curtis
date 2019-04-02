using Common.Model;

namespace Common.Service
{
    public interface IBasketService
    {
        Basket GetBasket(int basketId);
        Basket AddProductToBasket(int? basketId, int productId, int quantity);
        Basket RemoveProductFromBasket(int basketId, int productId);
        Basket UpdateProductQuantityInBasket(int basketId, int productId, int quantity);
        void DeleteBasket(int basketId);
    }
}
