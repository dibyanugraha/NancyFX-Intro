using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCartModule : NancyModule
    {
        public ShoppingCartModule(
            IShoppingCartStore shoppingCartStore,
            IProductCatalogClient productCatalog,
            IEventStore eventStore
        ) : base("/shoppingCart")
        {
            Get(
                "/{userid:int}", parameters => 
                {
                    var userId = (int) parameters.userid;
                    return shoppingCartStore.Get(userId);
                }
            );

            Post(
                "/{userid:int}/items",
                async(parameters, _) =>
                {
                    var productCatalogIds = this.Bind<int[]>();
                    var userId = (int) parameters.userId;

                    var shoppingCart = shoppingCartStore.Get(userId);
                    var shoppingCartItems = await
                        productCatalog
                            .GetShoppingCartItems(productCatalogIds)
                            .ConfigureAwait(false);
                    shoppingCart.AddItems(shoppingCartItems, eventStore);
                    shoppingCartStore.Save(shoppingCart);

                    return shoppingCart;
                }
            );

            Delete(
                "/{userid:int}/items", parameters =>
                {
                    var productCatalogIds = this.Bind<int[]>();
                    var userId = (int) parameters.userId;

                    var shoppingCart = shoppingCartStore.Get(userId);
                    shoppingCart.RemoveItems(productCatalogIds, eventStore);
                    shoppingCartStore.Save(shoppingCart);

                    return shoppingCart;
                }
            );
        }
    }
}