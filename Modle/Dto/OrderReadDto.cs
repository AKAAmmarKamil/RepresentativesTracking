using System;

namespace Dto
{
    public class OrderReadDto
    {
        public Guid ID { get; set; }
        public string Details { get; set; }
        public double? TotalPriceInIQD { get; set; }
        public double? TotalPriceInUSD { get; set; }
        public DateTimeOffset AddOrderDate { get; set; }
        public DateTimeOffset? DeliveryOrderDate { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double EndLatitude { get; set; }
        public string Status { get; set; }
        public UserReadDto User { get; set; }
        public CustomerForOrderReadDto Customer { get; set; }
    }
}
