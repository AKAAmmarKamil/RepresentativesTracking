using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class CompanyWriteDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double ExchangeRate { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public bool IsAcceptAutomaticCurrencyExchange { get; set; }
    }
}