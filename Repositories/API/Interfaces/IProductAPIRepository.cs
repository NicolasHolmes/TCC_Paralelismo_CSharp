using System.Net.Http;
using System.Threading.Tasks;

namespace Repositories.API.Interfaces
{
    public interface IProductAPIRepository
    {
        public Task<HttpResponseMessage> GetProductsAsync();

        public Task<HttpResponseMessage> GetProductDetailsAsync(int id);
    }
}
