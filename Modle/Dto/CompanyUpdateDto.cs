using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class CompanyUpdateDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public int RepresentativeCount { get; set; }
    }
}