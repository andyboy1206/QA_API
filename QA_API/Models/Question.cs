using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA_API.Models
{
    public class Question
    {
        [Key]
        [ForeignKey("QuestionId")]
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
