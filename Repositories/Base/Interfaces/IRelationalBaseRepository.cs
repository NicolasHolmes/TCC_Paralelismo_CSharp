using Models.SQLEntities;
using Models.SQLEntities.Base;
using Models.SQLEntities.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Base.Interfaces
{
    public interface IRelationalBaseRepository<Entity> where Entity : IBaseEntity
    {
        public Task<IEnumerable<Entity>> GetWhereAsync(object conditions);
        public Task<Entity> GetByIdAsync(int id);
        public Task<ForeignEntity> GetForeignEntityIdAsync<ForeignEntity>(int id) where ForeignEntity : BaseEntity, new();
        public Task<int?> AddAsync(Entity entity);
        public Task<bool> UpdateAsync(Entity entity);
        public Task<bool> SoftDeleteByIdAsync(int id);
    }
}
