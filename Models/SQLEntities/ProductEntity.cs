using Dapper;
using Models.SQLEntities.Base;

namespace Models.SQLEntities
{
    [Table("ProdutosVindosDaAPI")]
    public class ProductEntity : BaseEntity
    {
        public int IdEndpointProduct { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
    }
}
