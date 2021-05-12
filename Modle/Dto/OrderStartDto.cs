using System;
using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class OrderStartDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double StartLongitude { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double StartLatitude { get; set; }
    }
}
