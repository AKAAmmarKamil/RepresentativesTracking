using System;
using System.ComponentModel.DataAnnotations;
namespace Modle.Model
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RepresentativeCount { get; set; }
        public double ExchangeRate { get; set; }
        public bool IsAcceptAutomaticCurrencyExchange { get; set; }
    }
}