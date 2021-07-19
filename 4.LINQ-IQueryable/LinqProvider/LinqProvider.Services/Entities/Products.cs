using LinqProvider.Services.Models.Enum;

namespace LinqProvider.Services.Entities
{
    public class Products
    {
        [SqlColName("ProductID")]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityPerUnit { get; set; }
        public int UnitsInStock { get; set; }
    }
}
