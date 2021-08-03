using System;

namespace Dto
{
    public class LocationReadDto
    {
        public Guid ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTimeOffset LocationDate { get; set; }
        public bool IsOnline { get; set; }
        public OrderReadDto Order { get; set; }
    }
}
