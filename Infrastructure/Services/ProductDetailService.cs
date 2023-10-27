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

        public async Task<ProductDetailResponse> GetProductsDetailsResponseByApiAsync(int id, int requestNumber, int requestsQuantity, int timesItRan)
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
            productsDetailResponse.RequestsQuantity = requestsQuantity;
            productsDetailResponse.TimesItRan = timesItRan + 1;

            return productsDetailResponse;
        }

        public async Task<int> GetRequestsQuantity()
        {
            HttpResponseMessage response = await _productAPIRepository.GetRequestsQuantityAsync();
            // Método para desserialização do JSON 
            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }
            string responseContent = await response.Content.ReadAsStringAsync();

            int requestsQuantity = JsonConvert.DeserializeObject<int>(responseContent);
            return requestsQuantity;
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
                    TypeOfExtraction = response.TypeOfExtraction,
                    RequestsQuantity = response.RequestsQuantity,
                    TimesItRan = response.TimesItRan,
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

            int requestsQuantity = await GetRequestsQuantity();

            List<int> ids = await _productDBRepository.SelectIdsOfProductsAsync();

            int timesItRan = await _productDetailDBRepository.SelectTimesItRan(requestsQuantity);

            #region Thread
            //List<Thread> threads = new List<Thread>();
            //List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();
            //object lockObject = new object(); // Objeto de trava


            //foreach (int id in ids)
            //{
            //    requestNumber++;
            //    Thread thread = new Thread(() =>
            //    {
            //        try
            //        {
            //            ProductDetailResponse response = GetProductsDetailsResponseByApiAsync(id, requestNumber, requestsQuantity, timesItRan).Result;
            //            response.TypeOfExtraction = "Thread";

            //            lock (lockObject) // Garantindo que não haverá acesso simultâneo na lista
            //            {
            //                productsResponses.Add(response);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    });
            //    thread.Start();
            //    threads.Add(thread);
            //}
            //threads.ForEach(thread => thread.Join());

            #region BulkInsert
            //await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            //threads.Clear();
            #endregion

            #region Task
            //int maxParallelism = 16; // Defina o número máximo de tarefas paralelas que você deseja

            //List<Task> tasks = new List<Task>();
            //List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();
            //object lockObject = new object(); // Objeto de bloqueio


            //var semaphore = new SemaphoreSlim(maxParallelism);
            //foreach (int id in ids)
            //{
            //    await semaphore.WaitAsync(); // Aguarde para entrar na seção crítica (limitado pelo maxParallelism)
            //    requestNumber++;

            //    Task task = Task.Run(async () =>
            //    {
            //        try
            //        {
            //            ProductDetailResponse response = await GetProductsDetailsResponseByApiAsync(id, requestNumber, requestsQuantity, timesItRan);
            //            response.TypeOfExtraction = "Task";

            //            // Bloqueio para garantir acesso exclusivo à lista
            //            lock (lockObject)
            //            {
            //                productsResponses.Add(response);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //        finally
            //        {
            //            semaphore.Release(); // Libere a seção crítica
            //        }
            //    });

            //    tasks.Add(task);
            //}

            //await Task.WhenAll(tasks);

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
            //        ProductDetailResponse response = GetProductsDetailsResponseByApiAsync(id, requestNumber, requestsQuantity, timesItRan).Result;
            //        response.TypeOfExtraction = "Parallel";
            //        lock (productsResponses) // Trava
            //        {
            //            productsResponses.Add(response);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //});

            #region BulkInsert
            //await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            #endregion

            #region Sequential
            List<ProductDetailResponse> productsResponses = new List<ProductDetailResponse>();

            foreach (int id in ids)
            {
                requestNumber++;

                try
                {
                    ProductDetailResponse response = await GetProductsDetailsResponseByApiAsync(id, requestNumber, requestsQuantity, timesItRan);
                    response.TypeOfExtraction = "Sequential";
                    productsResponses.Add(response);
                }
                catch (Exception ex)
                {
                }
            }

            #region BulkInsert
            await BulkInsertProductsDetailsAsync(productsResponses);
            #endregion

            #endregion

            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}
