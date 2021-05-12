using System;
using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class OrderEndDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public string ReceiptImageUrl { get; set; }
    }
}
