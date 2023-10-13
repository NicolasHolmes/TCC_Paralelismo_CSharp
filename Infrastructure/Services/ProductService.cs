using Infrastructure.Extensions;
using Infrastructure.Services.Interfaces;
using Models.ExternalEntities;
using Models.SQLEntities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Repositories.API.Interfaces;
using Repositories.DataBase.Interfaces;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private IProductAPIRepository _productAPIRepository;
        private IProductDBRepository _productDBRepository;
        public ProductService(IProductAPIRepository productAPIRepository, IProductDBRepository productDBRepository)
        {
            _productAPIRepository = productAPIRepository;
            _productDBRepository = productDBRepository;
        }

        public async Task<ProductResponse> GetProductsResponseByApiAsync()
        {

            HttpResponseMessage response = await _productAPIRepository.GetProductsAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Método para desserialização do JSON 
            ProductResponse productsResponse = JsonConvert.DeserializeObject<ProductResponse>(responseContent);

            // TODO: Validar o que chegou null para não dar erro na transferência dos dados
            return productsResponse;
        }
        public async Task ProcessAsync()
        {
            List<Thread> threads = new List<Thread>();
            List<ProductResponse> productsResponses = new List<ProductResponse>();

            #region Thread
            Thread thread = new Thread(() =>
            {
                productsResponses.Add(GetProductsResponseByApiAsync().Result);
            });
            thread.Start();
            Task.Delay(100).Wait();
            threads.Add(thread);
            threads.ForEach(thread => thread.Join());

            foreach (ProductResponse response in productsResponses)
            {
                ProductEntity entity = new ProductEntity
                {
                    IdEndpointProduct = response.Id,
                    Name = response.Name,
                    StockQuantity = response.StockQuantity,
                };

                await _productDBRepository.InsertProductsBaseInfoAsync(entity);
            }

            threads.Clear();
            #endregion
            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}

