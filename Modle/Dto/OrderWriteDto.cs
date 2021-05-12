using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dto
{
    public class OrderWriteDto : IValidatableObject
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Details { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double EndLongitude { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double EndLatitude { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public int UserID { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (DBContext)validationContext.GetService(typeof(DBContext));
            var User = service.User.FirstOrDefault(x => x.ID == UserID);
            if (User == null)
            {
                yield return new ValidationResult("تعذر إدخال المستخدم");
            }
        }
    }
}
