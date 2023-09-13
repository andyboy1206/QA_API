using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA_API.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="A Tag Name is Required")]
        public string TagName { get; set; } = String.Empty;

        public int QuestionId { get; set; }
    }
}
