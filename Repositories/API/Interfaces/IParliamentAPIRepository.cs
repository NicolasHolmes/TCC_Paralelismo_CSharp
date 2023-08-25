using Models.SQLEntities;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repositories.API.Interfaces
{
    public interface IParliamentAPIRepository
    {
        public Task<HttpResponseMessage> GetParliamentDeputiesAsync();

        public Task<HttpResponseMessage> GetParliamentDeputyDetailsAsync(int id);
    }
}
