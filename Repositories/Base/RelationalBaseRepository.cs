using Dapper;
using Models.SQLEntities.Base;
using Models.SQLEntities.Base.Interfaces;
using Repositories.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repositories.Base
{
    public abstract class RelationalBaseRepository<Entity> : IRelationalBaseRepository<Entity> where Entity : IBaseEntity, new()
    {
        private IDbConnection _connection;
        public RelationalBaseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public virtual async Task<int?> AddAsync(Entity entity)
        {
            entity.CreationDate = DateTime.Now;
            return await _connection.InsertAsync(entity);
        }

        public virtual async Task<ForeignEntity> GetForeignEntityIdAsync<ForeignEntity>(int id) where ForeignEntity : BaseEntity, new()
        {
            return await _connection.GetAsync<ForeignEntity>(id);
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _connection.GetAsync<Entity>(id);
        }

        public virtual async Task<IEnumerable<Entity>> GetWhereAsync(object conditions)
        {
            return await _connection.GetListAsync<Entity>(conditions);
        }
    }
}
