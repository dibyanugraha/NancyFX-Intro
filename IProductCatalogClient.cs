using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>>
          GetShoppingCartItems(int[] productCatalogueIds);
    }
}