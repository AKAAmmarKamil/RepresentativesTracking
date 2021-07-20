using System;

namespace Dto
{
    public class OrderReadDto
    {
        public int ID { get; set; }
        public string Details { get; set; }
        public int Count { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public double? TotalPriceInIQD { get; set; }
        public double? TotalPriceInUSD { get; set; }
        public DateTimeOffset AddOrderDate { get; set; }
        public DateTimeOffset? DeliveryOrderDate { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double EndLatitude { get; set; }
        public bool ISInProgress { get; set; }
        public UserReadDto User { get; set; }
    }
}
