using System;
namespace Models.ExternalEntities
{
    public class ProductDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public DateTime? MaturityDate { get; set; }
        public int Barcode { get; set; }
        public int StockQuantity { get; set; }
    }
}
