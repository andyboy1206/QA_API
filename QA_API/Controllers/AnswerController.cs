using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_API.Data;
using QA_API.Models;

namespace QA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly APIDBContext _db;
        private readonly ILogger<AnswerController> _logger;

        public AnswerController(APIDBContext db, ILogger<AnswerController> logger)
        {
            _db = db;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetAnswers()
        {
            try
            {
                IEnumerable<Answer> Answers = _db.Answers;
                if (Answers.Count() == 0)
                { return NotFound("No Answers found"); }

                return Ok(Answers);
            }

            catch (Exception ex)
            { 
            return BadRequest(ex.Message);
            }
        }

        [HttpGet("{AnswerId}", Name = "GetAnswerbyID")]

        public IActionResult GetAnswers(int AnswerId)
        {
            try
            {
                if (AnswerId > 0)
                {
                    IEnumerable<Answer> Answers = _db.Answers;

                    var Answer = Answers.FirstOrDefault(c => c.Id == AnswerId);

                    if (Answer == null)
                    { return NotFound("No Answer found with specified Id"); }

                    return Ok(Answer);
                }

                else
                { return BadRequest("Id is not valid"); }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("questionId/{questionId}")]
        public IActionResult GetAnswersbyQuestionId(int questionId)
        {
            try
            {
                if (questionId > 0)
                {
                    IEnumerable<Answer> Answers = _db.Answers;

                    var Answer = Answers.Where(c => c.QuestionId == questionId);

                    if (Answer.Count() == 0)
                    { return NotFound("No Answers found with specified Question Id"); }

                    return Ok(Answer);
                }

                else
                { return BadRequest("Question Id is not valid"); }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddAnswer(Answer Answer)
        {
            try
            {
                if (Answer == null)
                {
                    return BadRequest();
                }

                var questionId = _db.Questions.Where(x => x.Id == Answer.QuestionId).FirstOrDefault();
                if (questionId == null)
                { return BadRequest("Specified Question Id does not exists"); }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Answers.Add(Answer);
                _db.SaveChanges();
                return CreatedAtRoute("GetAnswerbyID", new { AnswerId = Answer.Id }, Answer);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult UpdateAnswer(Answer Answer)
        {
            try
            {
                if (Answer == null)
                {
                    return BadRequest("Data is not present");
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Answers.Update(Answer);
                _db.SaveChanges();
                return CreatedAtRoute("GetAnswerbyID", new { AnswerId = Answer.Id }, Answer);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("vote/{type}/{answerId}")]
        public IActionResult UpVote(string type, int answerId)
        {
            try
            {

                    IEnumerable<Answer> Answers = _db.Answers;
                    var answer = Answers.Where(x => x.Id == answerId).FirstOrDefault();

                    if (answer == null)
                    {
                        return BadRequest("Not a valid answer Id was specified");
                    }

                    switch (type.ToLower())
                    {
                        case "up":
                            answer.UpVote += 1;
                            break;

                        case "down":
                            answer.DownVote += 1;
                            break;

                        default:
                            return BadRequest("Not a valid vote type");
                            break;
                    }

                    _db.Answers.Update(answer);
                    _db.SaveChanges();
                    return Ok();
                
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpDelete("{AnswerId}")]
        public IActionResult DeleteAnswer(int AnswerId)
        {
            try
            {
                if (AnswerId > 0)
                {

                    IEnumerable<Answer> Answers = _db.Answers;
                    var Answer = Answers.FirstOrDefault(c => c.Id == AnswerId);

                    if (Answer == null)
                    {
                        return BadRequest("No Answer found with specified Id");
                    }

                    else
                    {
                        _db.Answers.Remove(Answer);
                        _db.SaveChanges();
                    }

                    return Ok("Answer Removed Succesfully");
                }

                else
                {
                    return BadRequest("Please Specify a valid AnswerId");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("insert/fromFile")]
        public IActionResult UpdateAnswersFromFile()
        {
            try
            {
                var e = Directory.GetCurrentDirectory() + @"\Files\Answers.txt";
                StreamReader reader = new StreamReader(e);

                List<Answer> Answers = new List<Answer>();



                while (!reader.EndOfStream)
                {
                    string[] answerinfo = reader.ReadLine().Split(',');
                    _db.Answers.Add(new Answer { AnswerText = answerinfo[0], QuestionId = Convert.ToInt32(answerinfo[1]), UpVote = Convert.ToInt32(answerinfo[2]), DownVote = Convert.ToInt32(answerinfo[3]) });
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
