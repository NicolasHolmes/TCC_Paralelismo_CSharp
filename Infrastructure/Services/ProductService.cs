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

        public async Task<List<ProductResponse>> GetProductsResponseByApiAsync()
        {

            HttpResponseMessage response = await _productAPIRepository.GetProductsAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Método para desserialização do JSON 
            List<ProductResponse> productsResponse = JsonConvert.DeserializeObject<List<ProductResponse>>(responseContent);

            // TODO: Validar o que chegou null para não dar erro na transferência dos dados
            return productsResponse;
        }
        public async Task ProcessAsync()
        {
            List<ProductResponse> productsResponses = await GetProductsResponseByApiAsync();
            List<ProductEntity> entities = new List<ProductEntity>();

            foreach (ProductResponse response in productsResponses)
            {
                ProductEntity entity = new ProductEntity
                {
                    IdEndpointProduct = response.id,
                    Name = response.name,
                    StockQuantity = response.stockQuantity,
                };
                entities.Add(entity);
            }

            await _productDBRepository.BulkInsertProductsDetail(entities);

            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}

