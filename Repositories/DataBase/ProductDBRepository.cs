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
    public class ProductDBRepository : RelationalBaseRepository<ProductEntity>, IProductDBRepository
    {
        IDbConnectionExtractBot _connection;
        public ProductDBRepository(IDbConnectionExtractBot connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task InsertProductsBaseInfoAsync(ProductEntity productsEntity)
        {
            var query = new StringBuilder();

            query.Append("INSERT INTO [TCC].[dbo].[ProdutosVindosDaAPI] ")
                 .Append("([IdEndpointProduto], [Name], [StockQuantity], [CreationDate]) ")
                 .Append("VALUES");
            query.Append($"('{productsEntity.IdEndpointProduct}',");
            query.Append($"'{productsEntity.Name}',");
            query.Append($"'{productsEntity.StockQuantity}'");
            query.Append($"GETDATE())");

            try
            {
                await _connection.QueryAsync<ProductEntity>(query.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<int>> SelectIdsOfProductsAsync()
        {
            var query = ("SELECT [IdEndpointProduto] FROM [TCC].[dbo].[Produtos]");

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
