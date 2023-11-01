using Dapper;
using Models.SQLEntities;
using Repositories.Base;
using Repositories.Connections.Interfaces;
using Repositories.DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DataBase
{
    public class ProductDetailDBRepository : RelationalBaseRepository<ProductDetailEntity>, IProductDetailDBRepository
    {
        IDbConnectionExtractBot _connection;

        public ProductDetailDBRepository(IDbConnectionExtractBot connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task<int> SelectTimesItRan(int requestsQuantity, string typeOfExtraction)
        {
            try
            {
                var query = new StringBuilder();
                query.Append("SELECT MAX([TimesItRan]) FROM [TCC].[dbo].[DetalhesProdutosVindosDaAPI] ");
                query.Append("WHERE [RequestsQuantity] = @RequestsQuantity and [TypeOfExtraction] = @TypeOfExtraction");

                using (var connection = new SqlConnection(_connection.ConnectionString)) // Use _connection.ConnectionString para a string de conexão
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteScalarAsync<int>(query.ToString(), new { RequestsQuantity = requestsQuantity, TypeOfExtraction = typeOfExtraction });

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw; // Você pode considerar fazer um tratamento de exceção mais informativo aqui
            }
        }

        public async Task BulkInsertProductsDetail(IEnumerable<ProductDetailEntity> entities)
        {
            _connection.Open();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    // Define a consulta SQL para o Bulk Insert
                    string sql = "INSERT INTO [TCC].[dbo].[DetalhesProdutosVindosDaAPI] (IdEndpointProduct, Name, Description, Price, ExpirationDate, BarCode, StockQuantity, TypeOfExtraction, RequestsQuantity, TimesItRan, CreationDate) " +
                                         "VALUES (@IdEndpointProduct, @Name, @Description, @Price, @ExpirationDate, @BarCode, @StockQuantity, @TypeOfExtraction, @RequestsQuantity, @TimesItRan, @CreationDate)";

                    // Executa o Bulk Insert
                    await _connection.ExecuteAsync(sql, entities, transaction: transaction);

                    transaction.Commit();
                    Console.WriteLine($"Todas as responses foram salvas {DateTime.Now}");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Falha ao tentar salvar as responses, rollback feito {DateTime.Now}");
                    throw;
                }
            }
            _connection.Close();
        }
    }
}
