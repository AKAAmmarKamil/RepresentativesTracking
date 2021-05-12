using Modle.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DAL;
namespace Dto
{

    public class UserWriteDto : IValidatableObject
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [EmailAddress(ErrorMessage = "البريد الألكتروني غير صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [MinLength(8, ErrorMessage = "كلمة السر يجب ان تكون 8 أحرف كحد أدنى")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [Compare(nameof(Password), ErrorMessage = "كلمتا السر غير متطابقتان")]
        public string ReWritePassword { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Role { get; set; }
        [Range(1, 3, ErrorMessage = "نوع المندوب يجب أن يكون بين 1 و 3")]
        public int? Type { get; set; }
        public int? CompanyId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (DBContext)validationContext.GetService(typeof(DBContext));
            var EmailAddress = service.User.FirstOrDefault(x => x.Email == Email);
            var Company = service.Company.FirstOrDefault(x => x.Id == CompanyId);
            if (EmailAddress != null)
            {
                yield return new ValidationResult("البريد الألكتروني غير صحيح");
            }
            if (Role != "Admin" &&Role != "Representative" && Role != "DeliveryAdmin")
            {
                yield return new ValidationResult("الصلاحية غير صحيحة");
            }
            if (CompanyId == null && Role != "Admin")
            {
                yield return new ValidationResult("هذا الحقل مطلوب");
            }
            if (Company == null)
            {
                yield return new ValidationResult("الشركة غير موجودة");
            }
            if (PhoneNumber.Length != 11||PhoneNumber[0]!='0'|| PhoneNumber[1] != '7')
            {
                yield return new ValidationResult("رقم الهاتف غير صحيح");
            }
            if (Role == "Representative" && Type ==null)
            {
                yield return new ValidationResult("نوع المندوب مطلوب");
            }
            yield return ValidationResult.Success;
        }
    }
}