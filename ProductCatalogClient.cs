using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Polly;
using System.Linq;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        private static string productCatalogApiUrl = @"";
        private static string getProductPathTemplate = @"";

        private static async Task<HttpResponseMessage>
        RequestProductFromProductCatalogApi(int[] productCatalogIds)
        {
            var productResources = string.Format(
                getProductPathTemplate, string.Join(",", productCatalogIds)
            );

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(productCatalogApiUrl);
                return await
                    httpClient.GetAsync(productResources).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<ShoppingCartItem>>
          GetShoppingCartItems(int[] productCatalogueIds)
        {
            var response = await
                RequestProductFromProductCatalogApi(productCatalogueIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private static async Task<IEnumerable<ShoppingCartItem>>
            ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products = JsonConvert.DeserializeObject<List<ProductCatalogProduct>>
                (await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            return products.Select
                (q => new ShoppingCartItem(
                    int.Parse(q.ProductId),
                    q.ProductName,
                    q.ProductDescription,
                    q.Price
                ));
        }

        private class ProductCatalogProduct
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public Money Price { get; set; }

        }
    }

}