using Dapper;
using Models.SQLEntities.Base;
using System;

namespace Models.SQLEntities
{
    [Table("DetalhesProdutosVindosDaAPI")]
    public class ProductDetailEntity : BaseEntity
    {
        public int IdEndpointProduct { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long BarCode { get; set; }
        public int StockQuantity { get; set; }
        public string TypeOfExtraction { get; set; }
    }
}
