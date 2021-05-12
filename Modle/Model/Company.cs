using System.ComponentModel.DataAnnotations;
namespace Modle.Model
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
