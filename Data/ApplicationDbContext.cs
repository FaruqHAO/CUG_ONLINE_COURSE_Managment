using CUG_ONLINE_COURSES.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CUG_ONLINE_COURSES.Data
{
    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    //{
    //    public DbSet<Course> Courses { get; set; }
    //}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<StudentResultDto> StudentResult { get; set; }


    }
}
