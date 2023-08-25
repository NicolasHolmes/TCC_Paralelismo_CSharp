using Models.SQLEntities;
using Repositories.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.DataBase.Interfaces
{
    public interface IDeputyDBRepository : IRelationalBaseRepository<DeputiesEntity>
    {
        public Task InsertDeputiesBaseInfoAsync(DeputiesEntity deputiesEntity);

        public Task<List<int>> SelectIdsOfDeputiesAsync();
    }
}
