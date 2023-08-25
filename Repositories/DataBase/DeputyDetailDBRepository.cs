using Dapper;
using Models.SQLEntities;
using Repositories.Base;
using Repositories.Connections.Interfaces;
using Repositories.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Repositories.DataBase
{
    public class DeputyDetailDBRepository : RelationalBaseRepository<DeputiesDetailEntity>, IDeputyDetailDBRepository
    {
        IDbConnectionExtractBot _connection;
        public DeputyDetailDBRepository(IDbConnectionExtractBot connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task InsertDeputiesDetailAsync(DeputiesDetailEntity deputiesEntity, int requestNumber)
        {
            var query = new StringBuilder();

            query.Append("INSERT INTO [TCC].[dbo].[DeputadosDetalhes] ")
                 .Append("([IdEndpointDeputado], [NomeCivil], [Cpf], [Sexo], [DataNascimento], [UfNascimento], [MunicipioNascimento], [Escolaridade], [DataCriacao], [DataAtualizacao], [Deletado]) ")
                 .Append("VALUES");
            query.Append($"({deputiesEntity.IdEndpointDeputado},");
            query.Append($"'{deputiesEntity.NomeCivil}',");
            query.Append($"'{deputiesEntity.Cpf}',");
            query.Append($"'{deputiesEntity.Sexo}',");
            query.Append($"'{deputiesEntity.DataNascimento.ToString("yyyy-MM-dd")}',");
            query.Append($"'{deputiesEntity.UfNascimento}',");
            query.Append($"'{deputiesEntity.MunicipioNascimento}',");
            query.Append($"'{deputiesEntity.Escolaridade}',");
            query.Append($"GETDATE(),");
            query.Append($"GETDATE(),");
            query.Append($"0)");

            try
            {
                await _connection.QueryAsync<DeputiesDetailEntity>(query.ToString());
                Console.WriteLine($"Response da request {requestNumber} salva {DateTime.Now}");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task BulkInsertDeputiesDetail(IEnumerable<DeputiesDetailEntity> entities)
        {
            _connection.Open();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    // Define a consulta SQL para o Bulk Insert
                    string sql = "INSERT INTO [TCC].[dbo].[DeputadosDetalhes] (IdEndpointDeputado, NomeCivil, Cpf, Sexo, DataNascimento, UfNascimento, MunicipioNascimento, Escolaridade) " +
                                 "VALUES (@IdEndpointDeputado, @NomeCivil, @Cpf, @Sexo, @DataNascimento, @UfNascimento, @MunicipioNascimento, @Escolaridade)";

                    // Executa o Bulk Insert
                    await _connection.ExecuteAsync(sql, entities, transaction: transaction);

                    transaction.Commit();
                    Console.WriteLine($"Todas as reponses foram salvas {DateTime.Now}");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Falha ao tentar salvar as responses, rollback feito {DateTime.Now}");
                    throw;
                }
            }
        }
    }
}
