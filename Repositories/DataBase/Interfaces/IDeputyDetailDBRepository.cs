using Models.SQLEntities;
using Repositories.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.DataBase.Interfaces
{
    public interface IDeputyDetailDBRepository : IRelationalBaseRepository<DeputiesDetailEntity>
    {
        public Task InsertDeputiesDetailAsync(DeputiesDetailEntity deputiesDetailEntity, int requestNumber);

        public Task BulkInsertDeputiesDetail(IEnumerable<DeputiesDetailEntity> entities);

    }
}
