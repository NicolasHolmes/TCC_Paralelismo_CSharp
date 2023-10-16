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
    public class ProductDetailService : IProductDetailService
    {
        private IProductAPIRepository _productAPIRepository;
        private IProductDBRepository _productDBRepository;
        private IProductDetailDBRepository _productDetailDBRepository;
        public ProductDetailService(IProductAPIRepository productAPIRepository, IProductDBRepository productDBRepository, IProductDetailDBRepository productDetailDBRepository)
        {
            _productAPIRepository = productAPIRepository;
            _productDBRepository = productDBRepository;
            _productDetailDBRepository = productDetailDBRepository;
        }

        public async Task<ProductDetailResponse> GetProductsDetailsResponseByApiAsync(int id, int requestNumber)
        {
            HttpResponseMessage response = await _productAPIRepository.GetProductDetailsAsync(id);
            ConsoleExtension.WriteLog($"Request {requestNumber} feita {DateTime.Now}");
            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Método para desserialização do JSON 
            ProductDetailResponse productsDetailResponse = JsonConvert.DeserializeObject<ProductDetailResponse>(responseContent);
            productsDetailResponse.CreationDate = DateTime.Now;
            // TODO: Validar o que chegou null para não dar erro na transferência dos dados
            return productsDetailResponse;
        }

        public async Task SaveProductsResponsesOneByOneAsync(List<ProductDetailResponse> productsResponses)
        {
            int requestNumber = 0;

            foreach (ProductDetailResponse response in productsResponses)
            {
                requestNumber++;
                ProductDetailEntity entity = new ProductDetailEntity
                {
                    IdEndpointProduct = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    Price = response.Price,
                    ExpirationDate = response.ExpirationDate,
                    BarCode = response.BarCode,
                    StockQuantity = response.StockQuantity,
                    CreationDate = response.CreationDate
                };

                // Realize o Insert de cada request
                await _productDetailDBRepository.InsertProductsDetailAsync(entity, requestNumber);
            }
        }
        public async Task BulkInsertProductsDetailsAsync(List<ProductDetailResponse> productsResponses)
        {
            //Prepara a lista de entidades para o Bulk Insert
            List<ProductDetailEntity> entitiesToInsert = new List<ProductDetailEntity>();

            foreach (ProductDetailResponse response in productsResponses)
            {
                ProductDetailEntity entity = new ProductDetailEntity
                {
                    IdEndpointProduct = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    Price = response.Price,
                    ExpirationDate = response.ExpirationDate,
                    BarCode = response.BarCode,
                    StockQuantity = response.StockQuantity,
                    CreationDate = response.CreationDate
                };
                entitiesToInsert.Add(entity);
            }

            // Realize o Bulk Insert
            await _productDetailDBRepository.BulkInsertProductsDetail(entitiesToInsert);
        }

        public async Task ProcessAsync()
        {

            int requestNumber = 0;

            List<int> ids = await _productDBRepository.SelectIdsOfProductsAsync();

            #region Thread
            List<Thread> threads = new List<Thread>();
            List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();
            object lockObject = new object(); // Objeto de trava


            foreach (int id in ids)
            {
                requestNumber++;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        ProductDetailResponse response = GetProductsDetailsResponseByApiAsync(id, requestNumber).Result;

                        lock (lockObject) // Garantindo que não haverá acesso simultâneo na lista
                        {
                            productsResponses.Add(response);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
                thread.Start();
                Task.Delay(250).Wait();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());

            #region SalvandoUmPorUm
            //await SaveProductsResponsesOneByOneAsync(productsResponses);
            #endregion

            #region BulkInsert
            await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            //threads.Clear();
            #endregion

            #region Task
            //List<Task> tasks = new List<Task>();
            //List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();
            //object lockObject = new object(); // Objeto de bloqueio

            //foreach (int id in ids)
            //{
            //    requestNumber++;

            //    Task task = Task.Run(async () =>
            //    {
            //        try
            //        {
            //            ProductDetailResponse response = await GetProductDetailResponseByApiAsync(id, requestNumber);

            //            // Bloqueio para garantir acesso exclusivo à lista
            //            lock (lockObject)
            //            {
            //                productsResponses.Add(response);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    });

            //    tasks.Add(task);
            //    await Task.Delay(200);
            //}

            //await Task.WhenAll(tasks);

            #region SalvandoUmPorUm
            //await SaveProductsResponsesOneByOneAsync(productsResponses);
            #endregion

            #region BulkInsert
            //await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            #endregion

            #region Parallel
            //List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();

            //Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, id =>
            //{
            //    int currentRequestNumber = Interlocked.Increment(ref requestNumber);
            //    try
            //    {
            //        ProductDetailResponse response = GetProductsDetailsResponseByApiAsync(id, currentRequestNumber).Result;
            //        lock (productsResponses) // Trava
            //        {
            //            productsResponses.Add(response);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //});

            #region SalvandoUmPorUm
            //await SaveProductsResponsesOneByOneAsync(productsResponses);
            #endregion

            #region BulkInsert
            //await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            #endregion

            #region Sequential

            //List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();

            //foreach (int id in ids)
            //{
            //    requestNumber++;

            //    try
            //    {
            //        ProductDetailResponse response = await GetProductDetailResponseByApiAsync(id, requestNumber);
            //        productsResponses.Add(response);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            #region SalvandoUmPorUm
            //await SaveProductsResponsesOneByOneAsync(productsResponses);
            #endregion

            #region BulkInsert
            //await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            #endregion

            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}
