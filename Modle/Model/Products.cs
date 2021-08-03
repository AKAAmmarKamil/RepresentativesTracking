using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modle.Model
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}
