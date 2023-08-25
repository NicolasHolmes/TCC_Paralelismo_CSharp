using Dapper;
using Models.SQLEntities;
using Repositories.Base;
using Repositories.Connections.Interfaces;
using Repositories.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DataBase
{
    public class DeputyDBRepository : RelationalBaseRepository<DeputiesEntity>, IDeputyDBRepository
    {
        IDbConnectionExtractBot _connection;
        public DeputyDBRepository(IDbConnectionExtractBot connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task InsertDeputiesBaseInfoAsync(DeputiesEntity deputiesEntity)
        {
            var query = new StringBuilder();

            query.Append("INSERT INTO [TCC].[dbo].[Deputados] ")
                 .Append("([IdEndpointDeputado], [Nome], [SiglaPartido], [SiglaUf], [LegislaturaId], [Email], [DataCriacao], [DataAtualizacao], [Deletado]) ")
                 .Append("VALUES");
            query.Append($"('{deputiesEntity.IdEndpointDeputado}',");
            query.Append($"'{deputiesEntity.Nome}',");
            query.Append($"'{deputiesEntity.SiglaPartido}',");
            query.Append($"'{deputiesEntity.SiglaUf}',");
            query.Append($"{deputiesEntity.LegislaturaId},");
            query.Append($"'{deputiesEntity.Email}',");
            query.Append($"GETDATE(),");
            query.Append($"GETDATE(),");
            query.Append($"0)");

            try
            {
                await _connection.QueryAsync<DeputiesEntity>(query.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<int>> SelectIdsOfDeputiesAsync()
        {
            var query = ("SELECT [IdEndpointDeputado] FROM [TCC].[dbo].[Deputados]");

            try
            {
                var ids = await _connection.QueryAsync<int>(query);
                return ids.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
  