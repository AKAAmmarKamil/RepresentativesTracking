using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modle.Model
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public string Details { get; set; }
        public int Count { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public DateTimeOffset AddOrderDate { get; set; }
        public DateTimeOffset? DeliveryOrderDate { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double EndLatitude { get; set; }
        public bool ISInProgress { get; set; }
        public string? ReceiptImageUrl { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
