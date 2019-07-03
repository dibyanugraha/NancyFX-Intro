using Nancy;
using Nancy.ModelBinding;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCartModule : NancyModule
    {
        public ShoppingCartModule(
            IShoppingCartStore shoppingCartStore
        ) : base("/shoppingCart")
        {
            Get(
                "/{userid:int}", parameters => 
                {
                    var userId = (int) parameters.userid;
                    return shoppingCartStore.Get(userId);
                }
            );
        }
    }
}