using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA_API.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; } = String.Empty;

        public int UpVote { get; set; }

        public int DownVote { get; set; }

      
        public DateTime CreatedOn { get; set; }

        public int QuestionId { get; set; }
    }
}
