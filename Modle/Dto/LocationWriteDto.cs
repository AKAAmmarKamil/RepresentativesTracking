using System.ComponentModel.DataAnnotations;
namespace Dto
{
    public class LocationWriteDto
    {
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "لا يمكنك ترك هذا الحقل فارغاً")]
        public double Latitude { get; set; }
    }
}
