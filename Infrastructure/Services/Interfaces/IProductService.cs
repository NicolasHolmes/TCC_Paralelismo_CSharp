using Infrastructure.Services.Base.Interfaces;
using Models.ExternalEntities;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IProductService : IExtractService
    {
        public Task<ProductResponse> GetProductsResponseByApiAsync();
    }
}
