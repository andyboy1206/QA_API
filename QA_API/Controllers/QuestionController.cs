using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_API.Data;
using QA_API.Models;

namespace QA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly APIDBContext _db;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(APIDBContext db, ILogger<QuestionController> logger)
        {
            _db = db;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetQuestions()
        {
            try
            {
                IEnumerable<Question> questions = _db.Questions;
                if (questions.Count() == 0)
                { return NotFound("No questions found"); }

                return Ok(questions);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{questionId}", Name = "GetquestionbyID")]

        public IActionResult GetQuestions(int questionId)
        {
            try
            {
                if (questionId > 0)
                {
                    IEnumerable<Question> questions = _db.Questions;

                    var question = questions.FirstOrDefault(c => c.Id == questionId);

                    if (question == null)
                    { return NotFound("No question found with specified Id"); }

                    return Ok(question);
                }

                else
                { return BadRequest("Id is not valid"); }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Addquestion(Question question)
        {
            try
            {
                if (question == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Questions.Add(question);
                _db.SaveChanges();
                return CreatedAtRoute("GetquestionbyID", new { questionId = question.Id }, question);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Updatequestion(Question question)
        {
            try
            {
                if (question == null)
                {
                    return BadRequest("Data is not present");
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Questions.Update(question);
                _db.SaveChanges();
                return CreatedAtRoute("GetquestionbyID", new { questionId = question.Id }, question);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{questionId}")]
        public IActionResult Deletequestion(int questionId)
        {
            try
            {
                if (questionId > 0)
                {

                    IEnumerable<Question> questions = _db.Questions;
                    var question = questions.FirstOrDefault(c => c.Id == questionId);

                    if (question == null)
                    {
                        return BadRequest("No question found with specified Id");
                    }

                    else
                    {
                        _db.Questions.Remove(question);
                        _db.SaveChanges();
                    }

                    return Ok("Question Removed Succesfully");
                }

                else
                {
                    return BadRequest("Please Specify a valid questionId");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("insert/fromFile")]
        public IActionResult UpdateQuestionsFromFile()
        {
            try
            {
                var e = Directory.GetCurrentDirectory() + @"\Files\Questions.txt";
                StreamReader reader = new StreamReader(e);

                List<Question> questions = new List<Question>();

                while (!reader.EndOfStream)
                {
                    _db.Questions.Add(new Question { QuestionText = reader.ReadLine(), CreatedOn = DateTime.Now });
                    _db.SaveChanges();
                }

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
