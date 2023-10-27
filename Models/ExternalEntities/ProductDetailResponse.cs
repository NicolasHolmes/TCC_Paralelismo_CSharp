using System;
namespace Models.ExternalEntities
{
    public class ProductDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long BarCode { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? CreationDate { get; set; }
        public string TypeOfExtraction { get; set; }
        public int RequestsQuantity { get; set; }
        public int TimesItRan { get; set; }

    }
}
