using System;
using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class OrderDeliveryDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string ReceiptImageUrl { get; set; }
    }
}
