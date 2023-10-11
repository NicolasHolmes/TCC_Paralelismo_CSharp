using Models.SQLEntities;
using Repositories.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.DataBase.Interfaces
{
    public interface IProductDetailDBRepository : IRelationalBaseRepository<ProductDetailEntity>
    {
        public Task InsertProductsDetailAsync(ProductDetailEntity productDetailEntity, int requestNumber);

        public Task BulkInsertProductsDetail(IEnumerable<ProductDetailEntity> entities);

    }
}
