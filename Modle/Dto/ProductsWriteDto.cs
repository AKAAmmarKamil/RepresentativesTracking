using DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Dto
{
    public class ProductsWriteDto : IValidatableObject
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public int Count { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public int OrderID { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (DBContext)validationContext.GetService(typeof(DBContext));
            var User = service.Order.FirstOrDefault(x => x.ID == OrderID);
            if (User == null)
            {
                yield return new ValidationResult("الطلب غير موجود");
            }
        }
    }
}