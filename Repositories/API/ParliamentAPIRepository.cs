using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Repositories.API.Interfaces;

namespace Repositories.API
{
    public class ParliamentAPIRepository : IParliamentAPIRepository
    {
        public async Task<HttpResponseMessage> GetParliamentDeputiesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://dadosabertos.camara.leg.br/api/v2/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponseMessage = await client.GetAsync("deputados?ordem=ASC&ordenarPor=nome");

                return httpResponseMessage;
            }
        }

        public async Task<HttpResponseMessage> GetParliamentDeputyDetailsAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"https://dadosabertos.camara.leg.br/api/v2/deputados/{id}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponseMessage = await client.GetAsync("");

                return httpResponseMessage;
            }
        }
    }
}
