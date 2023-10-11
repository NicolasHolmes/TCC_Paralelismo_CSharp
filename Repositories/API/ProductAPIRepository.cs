using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Repositories.API.Interfaces;

namespace Repositories.API
{
    public class ProductAPIRepository : IProductAPIRepository
    {
        public async Task<HttpResponseMessage> GetProductsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/api/Products");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponseMessage = await client.GetAsync("");

                return httpResponseMessage;
            }
        }

        public async Task<HttpResponseMessage> GetProductDetailsAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"https://localhost:5001/api/Products/{id}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponseMessage = await client.GetAsync("");

                return httpResponseMessage;
            }
        }
    }
}
