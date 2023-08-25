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
    public class DeputyService : IDeputyService
    {
        private IParliamentAPIRepository _parliamentAPIRepository;
        private IDeputyDBRepository _deputyDBRepository;
        public DeputyService(IParliamentAPIRepository parliamentAPIRepository, IDeputyDBRepository deputyDBRepository)
        {
            _parliamentAPIRepository = parliamentAPIRepository;
            _deputyDBRepository = deputyDBRepository;
        }

        public async Task<DeputiesResponse> GetDeputiesResponseByApiAsync()
        {

            HttpResponseMessage response = await _parliamentAPIRepository.GetParliamentDeputiesAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Lida com a resposta não bem-sucedida, se necessário
                throw new Exception($"Request failed with status code {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Método para desserialização do JSON 
            DeputiesResponse deputiesResponse = JsonConvert.DeserializeObject<DeputiesResponse>(responseContent);

            // TODO: Validar o que chegou null para não dar erro na transferência dos dados
            return deputiesResponse;
        }
        public async Task ProcessAsync()
        {
            List<Thread> threads = new List<Thread>();
            List<DeputiesResponse> deputiesResponses = new List<DeputiesResponse>();

            #region Thread
            Thread thread = new Thread(() =>
            {
                deputiesResponses.Add(GetDeputiesResponseByApiAsync().Result);
            });
            thread.Start();
            Task.Delay(100).Wait();
            threads.Add(thread);
            threads.ForEach(thread => thread.Join());

            foreach (DeputiesResponse response in deputiesResponses)
            {
                foreach (DeputiesList deputiesResponse in response.dados)
                {
                    DeputiesEntity entity = new DeputiesEntity
                    {
                        IdEndpointDeputado = deputiesResponse.id,
                        Nome = deputiesResponse.nome,
                        SiglaPartido = deputiesResponse.siglaPartido,
                        SiglaUf = deputiesResponse.siglaUf,
                        LegislaturaId = deputiesResponse.idLegislatura,
                        Email = deputiesResponse.email
                    };

                    await _deputyDBRepository.InsertDeputiesBaseInfoAsync(entity);
                }
            }

            threads.Clear();
            #endregion
            ConsoleExtension.WriteNotification($"{DateTime.Now}: Concluído!");
        }
    }
}

