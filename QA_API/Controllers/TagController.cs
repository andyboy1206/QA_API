using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_API.Data;
using QA_API.Models;

namespace QA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly APIDBContext _db;
        private readonly ILogger<TagController> _logger;

        public TagController(APIDBContext db, ILogger<TagController> logger)
        {
            _db = db;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetTags()
        {
            try
            {
                IEnumerable<Tag> Tags = _db.Tags;
                if (Tags.Count() == 0)
                { return NotFound("No Tags found"); }

                return Ok(Tags);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{TagId}", Name = "GetTagbyID")]

        public IActionResult GetTags(int TagId)
        {
            try
            {
                if (TagId > 0)
                {
                    IEnumerable<Tag> Tags = _db.Tags;

                    var Tag = Tags.FirstOrDefault(c => c.Id == TagId);

                    if (Tag == null)
                    { return NotFound("No Tag found with specified Id"); }

                    return Ok(Tag);
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
        public IActionResult GetTagsbyQuestionId(int questionId)
        {
            try
            {
                if (questionId > 0)
                {
                    IEnumerable<Tag> Tags = _db.Tags;

                    var Tag = Tags.Where(c => c.QuestionId == questionId);

                    if (Tag.Count() == 0)
                    { return NotFound("No Tags found with specified Question Id"); }

                    return Ok(Tag);
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
        public IActionResult AddTag(Tag Tag)
        {
            try
            {
                if (Tag == null)
                {
                    return BadRequest();
                }

                var questionId = _db.Questions.Where(x => x.Id == Tag.QuestionId).FirstOrDefault();
                if (questionId == null)
                { return BadRequest("Specified Question Id does not exists"); }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Tags.Add(Tag);
                _db.SaveChanges();
                return CreatedAtRoute("GetTagbyID", new { TagId = Tag.Id }, Tag);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult UpdateTag(Tag Tag)
        {
            try
            {
                if (Tag == null)
                {
                    return BadRequest("Data is not present");
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Tags.Update(Tag);
                _db.SaveChanges();
                return CreatedAtRoute("GetTagbyID", new { TagId = Tag.Id }, Tag);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{TagId}")]
        public IActionResult DeleteTag(int TagId)
        {
            try
            {
                if (TagId > 0)
                {

                    IEnumerable<Tag> Tags = _db.Tags;
                    var Tag = Tags.FirstOrDefault(c => c.Id == TagId);

                    if (Tag == null)
                    {
                        return BadRequest("No Tag found with specified Id");
                    }

                    else
                    {
                        _db.Tags.Remove(Tag);
                        _db.SaveChanges();
                    }

                    return Ok("Tag Removed Succesfully");
                }

                else
                {
                    return BadRequest("Please Specify a valid TagId");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        
    }
}


