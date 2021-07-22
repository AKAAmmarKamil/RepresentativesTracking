namespace Dto
{
    public class CompanyReadDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RepresentativeCount { get; set; }
        public double ExchangeRate { get; set; }
        public bool IsAcceptAutomaticCurrencyExchange { get; set; }
    }
}