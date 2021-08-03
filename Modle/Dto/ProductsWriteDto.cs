using DAL;
using System;
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
        public int Quantity { get; set; }
        public double? PriceInIQD { get; set; }
        public double? PriceInUSD { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public Guid OrderID { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var service = (DBContext)validationContext.GetService(typeof(DBContext));
            var Order = service.Order.FirstOrDefault(x => x.ID == OrderID);
            if (Order == null)
            {
                yield return new ValidationResult("الطلب غير موجود");
            }
            if (Quantity <=0)
            {
                yield return new ValidationResult("يجب أن تكون الكمية أكبر من صفر");
            }
            if (PriceInIQD <= 0 || PriceInUSD <=0)
            {
                yield return new ValidationResult("يجب أن يكون السعر أكبر من صفر");
            }
            if (PriceInIQD ==null && PriceInUSD == null)
            {
                yield return new ValidationResult("لا يمكن إضافة منتج بدون سعر");
            }
        }
    }
}