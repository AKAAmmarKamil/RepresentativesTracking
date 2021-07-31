using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class CustomerWriteDto : IValidatableObject
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string PhoneNumber { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PhoneNumber.Length != 11 || PhoneNumber[0] != '0' || PhoneNumber[1] != '7')
                yield return new ValidationResult("رقم الهاتف غير صحيح");
        }
    }
}
