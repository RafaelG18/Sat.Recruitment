using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.Models
{
    /// <summary>
    /// Represents user model
    /// </summary>
    public class UserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int? UserTypeId { get; set; }

        public decimal Money { get; set; }
    }
}