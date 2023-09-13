using Microsoft.EntityFrameworkCore;
using QA_API.Models;

namespace QA_API.Data
{
    public class APIDBContext:DbContext
    {
     

        public APIDBContext(DbContextOptions<APIDBContext> options) : base(options)
        {

        }


        public DbSet<User> Users{ get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers{ get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}

