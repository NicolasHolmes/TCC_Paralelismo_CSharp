using Models.SQLEntities;
using Repositories.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.DataBase.Interfaces
{
    public interface IProductDBRepository : IRelationalBaseRepository<ProductEntity>
    {
        public Task InsertProductsBaseInfoAsync(ProductEntity productiesEntity);

        public Task<List<int>> SelectIdsOfProductsAsync();

        public Task BulkInsertProductsDetail(IEnumerable<ProductEntity> productEntity);
    }
}
