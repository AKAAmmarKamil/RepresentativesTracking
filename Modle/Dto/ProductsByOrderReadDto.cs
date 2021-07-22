namespace Dto
{
    public class ProductsByOrderReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        public double? TotalPriceInIQD { get; set; }
        public double? TotalPriceInUSD { get; set; }
    }
}