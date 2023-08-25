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
    public class DeputyDetailService : IDeputyDetailService
    {
        private IParliamentAPIRepository _parliamentAPIRepository;
        private IDeputyDBRepository _deputyDBRepository;
        private IDeputyDetailDBRepository _deputyDetailDBRepository;
        public DeputyDetailService(IParliamentAPIRepository parliamentAPIRepository, IDeputyDBRepository deputyDBRepository, IDeputyDetailDBRepository deputyDetailDBRepository)
        {
            _parliamentAPIRepository = parliamentAPIRepository;
            _deputyDBRepository = deputyDBRepository;
            _deputyDetailDBRepository = deputyDetailDBRepository;
        }

        public async Task<DeputiesDetailResponse> GetDeputiesDetailResponseByApiAsync(int id, int requestNumber)
        {
            HttpResponseMessage response = await _parliamentAPIRepository.GetParliamentDeputyDetailsAsync(id);
            ConsoleExtension.WriteLog($"Request {requestNumber} feita {DateTime.Now}");
            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Método para desserialização do JSON 
            DeputiesDetailResponse deputiesDetailResponse = JsonConvert.DeserializeObject<DeputiesDetailResponse>(responseContent);

            // TODO: Validar o que chegou null para não dar erro na transferência dos dados
            return deputiesDetailResponse;
        }

        public async Task ProcessAsync()
        {

            int requestNumber = 0;

            List<int> ids = await _deputyDBRepository.SelectIdsOfDeputiesAsync();

            #region Thread
            List<Thread> threads = new List<Thread>();
            List<DeputiesDetailResponse> deputiesResponses = new List<DeputiesDetailResponse>();

            foreach (int id in ids)
            {
                requestNumber++;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        deputiesResponses.Add(GetDeputiesDetailResponseByApiAsync(id, requestNumber).Result);
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
            //requestNumber = 0;
            //foreach (DeputiesDetailResponse response in deputiesResponses)
            //{
            //    requestNumber++;
            //    DeputiesDetailEntity entity = new DeputiesDetailEntity
            //    {
            //        IdEndpointDeputado = response.dados.id,
            //        NomeCivil = response.dados.nomeCivil,
            //        Cpf = response.dados.cpf,
            //        Sexo = response.dados.sexo,
            //        DataNascimento = response.dados.dataNascimento,
            //        UfNascimento = response.dados.ufNascimento,
            //        MunicipioNascimento = response.dados.municipioNascimento,
            //        Escolaridade = response.dados.escolaridade
            //    };
            //    await _deputyDetailDBRepository.InsertDeputiesDetailAsync(entity, requestNumber);
            //}
            //threads.Clear();
            #endregion

            #region BulkInsert
            //Prepare a lista de entidades para o Bulk Insert
            List<DeputiesDetailEntity> entitiesToInsert = new List<DeputiesDetailEntity>();
            foreach (DeputiesDetailResponse response in deputiesResponses)
            {
                DeputiesDetailEntity entity = new DeputiesDetailEntity
                {
                    IdEndpointDeputado = response.dados.id,
                    NomeCivil = response.dados.nomeCivil,
                    Cpf = response.dados.cpf,
                    Sexo = response.dados.sexo,
                    DataNascimento = response.dados.dataNascimento,
                    UfNascimento = response.dados.ufNascimento,
                    MunicipioNascimento = response.dados.municipioNascimento,
                    Escolaridade = response.dados.escolaridade
                };
                entitiesToInsert.Add(entity);
            }

            // Realiza o Bulk Insert
            await _deputyDetailDBRepository.BulkInsertDeputiesDetail(entitiesToInsert);
            threads.Clear();
            #endregion

            #endregion

            #region Task
            //List<Task> tasks = new List<Task>();
            //List<DeputiesDetailResponse> deputiesResponses = new List<DeputiesDetailResponse>();
            //foreach (int id in ids)
            //{
            //    requestNumber++;

            //    Task task = Task.Run(async () =>
            //    {
            //        try
            //        {
            //            DeputiesDetailResponse response = await GetDeputiesDetailResponseByApiAsync(id, requestNumber);
            //            deputiesResponses.Add(response);            
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    });

            //    tasks.Add(task);
            //    await Task.Delay(3);
            //}

            //await Task.WhenAll(tasks);

            #region SalvandoUmPorUm
            //requestNumber = 0;
            //foreach (DeputiesDetailResponse response in deputiesResponses)
            //{
            //    requestNumber++;
            //    DeputiesDetailEntity entity = new DeputiesDetailEntity
            //    {
            //        IdEndpointDeputado = response.dados.id,
            //        NomeCivil = response.dados.nomeCivil,
            //        Cpf = response.dados.cpf,
            //        Sexo = response.dados.sexo,
            //        DataNascimento = response.dados.dataNascimento,
            //        UfNascimento = response.dados.ufNascimento,
            //        MunicipioNascimento = response.dados.municipioNascimento,
            //        Escolaridade = response.dados.escolaridade
            //    };

            //await _deputyDetailDBRepository.InsertDeputiesDetailAsync(entity, requestNumber);      
            //}
            #endregion

            #region BulkInsert
            // Prepare a lista de entidades para o Bulk Insert
            //List<DeputiesDetailEntity> entitiesToInsert = new List<DeputiesDetailEntity>();
            //foreach (DeputiesDetailResponse response in deputiesResponses)
            //{
            //    DeputiesDetailEntity entity = new DeputiesDetailEntity
            //    {
            //        IdEndpointDeputado = response.dados.id,
            //        NomeCivil = response.dados.nomeCivil,
            //        Cpf = response.dados.cpf,
            //        Sexo = response.dados.sexo,
            //        DataNascimento = response.dados.dataNascimento,
            //        UfNascimento = response.dados.ufNascimento,
            //        MunicipioNascimento = response.dados.municipioNascimento,
            //        Escolaridade = response.dados.escolaridade
            //    };
            //    entitiesToInsert.Add(entity);
            //}

            // Realize o Bulk Insert usando o Entity Framework Core
            //await _deputyDetailDBRepository.BulkInsertDeputiesDetail(entitiesToInsert);
            #endregion

            #endregion


            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}
