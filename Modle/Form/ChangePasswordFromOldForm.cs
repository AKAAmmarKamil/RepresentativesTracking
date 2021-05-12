using System.ComponentModel.DataAnnotations;

namespace Model.Form
{
    public class ChangePasswordFromOldForm
    {
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [MinLength(8, ErrorMessage = "كلمة السر يجب ان تكون 8 أحرف كحد أدنى")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        [Compare(nameof(NewPassword), ErrorMessage = "كلمتا السر غير متطابقتان")]
        public string ReWritePassword { get; set; }
    }
}
