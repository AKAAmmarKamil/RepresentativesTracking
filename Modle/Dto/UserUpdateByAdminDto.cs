using DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dto
{
    public class UserUpdateByAdminDto : IValidatableObject
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public int Type { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (DBContext)validationContext.GetService(typeof(DBContext));
            var EmailAddress = service.User.FirstOrDefault(x => x.Email == Email);
            if (EmailAddress != null)
            {
                yield return new ValidationResult("البريد الألكتروني غير صحيح");
            }
            if (PhoneNumber.Length != 11 || PhoneNumber[0] != '0' || PhoneNumber[1] != '7')
            {
                yield return new ValidationResult("رقم الهاتف غير صحيح");
            }
            yield return ValidationResult.Success;
        }
    }
}