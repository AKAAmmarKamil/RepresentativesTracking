using System;
using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class OrderEndDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [Range(0,3,ErrorMessage ="يجب أن يكون رمز الحالة بين 0 و 3")]
        public int Status { get; set; }
    }
}
