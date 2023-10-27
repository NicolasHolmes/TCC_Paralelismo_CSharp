using Infrastructure.Services.Base.Interfaces;
using Models.ExternalEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IProductDetailService : IExtractService
    {
        public Task<ProductDetailResponse> GetProductsDetailsResponseByApiAsync(int id, int requestNumber, int requestsQuantity, int timesItRan);
        public Task<int> GetRequestsQuantity();
        public Task BulkInsertProductsDetailsAsync(List<ProductDetailResponse> productsResponses);
    }
}
