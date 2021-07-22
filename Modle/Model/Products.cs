using System.ComponentModel.DataAnnotations.Schema;
namespace Modle.Model
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}
