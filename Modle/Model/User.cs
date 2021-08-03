using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modle.Model
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Activated { get; set; }
        public int? Type { get; set; }
        public Guid? CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
    }
}