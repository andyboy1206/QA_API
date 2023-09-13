using System.ComponentModel.DataAnnotations;

namespace QA_API.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string name { get; set; } = String.Empty;
        
        [EmailAddress(ErrorMessage = "A valid email Address is required.")]
        public string email { get; set; } = String.Empty;

        [MinLength(8,ErrorMessage = "Username must have at least 8 characters.")]
        public string userName { get; set; } = String.Empty;
    }
}
