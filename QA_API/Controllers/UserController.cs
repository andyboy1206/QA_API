using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_API.Data;
using QA_API.Models;

namespace QA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly APIDBContext _db;
        private readonly ILogger<UserController> _logger;

        public UserController(APIDBContext db, ILogger<UserController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                IEnumerable<User> users = _db.Users;
                if (users.Count() == 0)
                { return NotFound("No users found"); }

                return Ok(users);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}", Name ="GetUserbyID")]

        public IActionResult GetUsers(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    IEnumerable<User> users = _db.Users;

                    var user = users.FirstOrDefault(c => c.id == userId);

                    if (user == null)
                    { return NotFound("No user found with specified Id"); }

                    return Ok(user);
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
        public IActionResult AddUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Users.Add(user);
                _db.SaveChanges();
                return CreatedAtRoute("GetUserbyID", new { userId = user.id }, user);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Data is not present");
                }

                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                _db.Users.Update(user);
                _db.SaveChanges();
                return CreatedAtRoute("GetUserbyID", new { userId = user.id }, user);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                if (userId > 0)
                {

                    IEnumerable<User> users = _db.Users;
                    var user = users.FirstOrDefault(c => c.id == userId);

                    if (user == null)
                    {
                        return BadRequest("No user found with specified Id");
                    }

                    else
                    {
                        _db.Users.Remove(user);
                        _db.SaveChanges();
                    }

                    return Ok("User Removed Succesfully");
                }

                else
                {
                    return BadRequest("Please Specify a valid UserId");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        

    }
}
