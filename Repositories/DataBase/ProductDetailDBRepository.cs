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
    public class ProductDetailDBRepository : RelationalBaseRepository<ProductDetailEntity>, IProductDetailDBRepository
    {
        IDbConnectionExtractBot _connection;
        public ProductDetailDBRepository(IDbConnectionExtractBot connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task InsertProductsDetailAsync(ProductDetailEntity productEntity, int requestNumber)
        {
            var query = new StringBuilder();

            query.Append("INSERT INTO [TCC].[dbo].[DetalhesProdutosVindosDaAPI] ")
                 .Append("([IdEndpointProduct], [Name], [Description], [Price], [ExpirationDate], [BarCode], [StockQuantity], [CreationDate]) ")
                 .Append("VALUES");
            query.Append($"({productEntity.IdEndpointProduct},");
            query.Append($"'{productEntity.Name}',");
            query.Append($"'{productEntity.Description}',");
            query.Append($"'{productEntity.Price}',");
            query.Append($"'{productEntity.ExpirationDate}',");
            query.Append($"'{productEntity.BarCode}',");
            query.Append($"'{productEntity.StockQuantity}',");
            query.Append($"GETDATE())");

            try
            {
                await _connection.QueryAsync<ProductDetailEntity>(query.ToString());
                Console.WriteLine($"Response da request {requestNumber} salva {DateTime.Now}");
            }
            catch (Exception ex)
            {

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
                    string sql = "INSERT INTO [TCC].[dbo].[ProdutosDetalhes] (IdEndpointProduct, Name, Description, Price, ExpirationDate, BarCode, StockQuantity, CreationDate) " +
                                 "VALUES (@IdEndpointProduct, @Name, @Description, @Price, @ExpirationDate, @BarCode, @StockQuantity, @CreationDate)";

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
