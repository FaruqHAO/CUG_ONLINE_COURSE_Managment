using Microsoft.AspNetCore.Identity;

namespace CUG_ONLINE_COURSES.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? createdOn { get; set; }
        public bool status { get; set; }

    }
    
}
