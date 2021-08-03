using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modle.Model
{
    public class RepresentativeLocation
    {
        [Key]
        public Guid ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTimeOffset LocationDate { get; set; }
        public bool IsOnline { get; set; }
        public Guid UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public Guid? OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}
