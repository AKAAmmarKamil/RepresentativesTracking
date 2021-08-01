using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modle.Model
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public string Details { get; set; }
        public DateTimeOffset AddOrderDate { get; set; }
        public DateTimeOffset? DeliveryOrderDate { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double EndLatitude { get; set; }
        public int Status { get; set; }
        public string ReceiptImageUrl { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public virtual List<Products> Products { get; set; }
    }
}
