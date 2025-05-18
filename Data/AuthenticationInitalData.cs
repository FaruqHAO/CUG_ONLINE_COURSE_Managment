using Microsoft.AspNetCore.Identity;

namespace CUG_ONLINE_COURSES.Data
{
    public static class AuthenticationInitalData
    {
        public const string SuperAdminRole = "Superadmin";
        public const string SecretaryRole = "Secretary";
        public const string LecturerRole = "Lecturer";
        public const string StudentRole = "Student";
        public const string DeanRole = "Dean";
        public static readonly List<IdentityRole> AppRoles = new List<IdentityRole>
                    {
                        new IdentityRole
                        {
                            Id = "1",
                            Name = SuperAdminRole,
                            NormalizedName = SuperAdminRole,
                        },
                        new IdentityRole
                        {
                            Id = "2",
                            Name = SecretaryRole,
                            NormalizedName = SecretaryRole,
                        },
                        new IdentityRole
                        {
                            Id = "3",
                            Name = LecturerRole,
                            NormalizedName = LecturerRole,
                        },
                        new IdentityRole
                        {
                            Id = "4",
                            Name = StudentRole,
                            NormalizedName = StudentRole,
                        },
                        new IdentityRole
                        {
                            Id = "5",
                            Name = DeanRole,
                            NormalizedName = DeanRole,
                        },
                    };
    }

}
