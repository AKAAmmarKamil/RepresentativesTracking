using System;

namespace Dto
{
    public class ProductsReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Quantity { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public double? TotalPriceInIQD { get; set; }
        public double? TotalPriceInUSD { get; set; }
        public OrderReadDto Order { get; set; }
    }
}