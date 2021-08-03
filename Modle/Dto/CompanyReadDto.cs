using System;

namespace Dto
{
    public class CompanyReadDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int RepresentativeCount { get; set; }
        public double ExchangeRate { get; set; }
        public bool IsAcceptAutomaticCurrencyExchange { get; set; }
    }
}