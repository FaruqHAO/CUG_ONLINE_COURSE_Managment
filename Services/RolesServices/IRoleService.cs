using CUG_ONLINE_COURSES.Data;
using CUG_ONLINE_COURSES.Models;

namespace CUG_ONLINE_COURSES.Services.RolesServices
{

    public interface IRoleService
    {
        Task EnsureRolesExistAsync();
        Task<List<Admin>> GetAllAdmin();
        Task<List<Admin>> GetAllStaff();
        Task<List<StudentDto>> GetStudentsRegisteredForCourse(string courseId);
        Task<List<UserWithRoles>> GetUsersWithRolesAsync();
        Task<UserWithRoles> GetUsersWithRolesByUserAsync(ApplicationUser user);
        Task<bool> SaveUserRoles(UserWithRoles user);
        Task<bool> AddCourse(Course course);
        Task<List<Course>> GetAllCoursesAsync();

        Task<List<Course>> GetAllFacultyCoursesAsync(string facultyID);
        Task<List<LibraryItem>> GetLibraryItemsAsync();
        Task<bool> AddLibraryItemAsync(LibraryItem item);
        Task<Dashboard_Data> DashboardData();
        Task<List<staffDashboad>> DashboardDataStaff(string UserID);
        Task<bool> RegisterStudentForCourse(string studentId, int courseId);
        Task<List<Course>> GetStudentRegisteredCourses(string studentId);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task <bool>EditCourse(Course course);
        Task<bool> AddStudentResultAsync(StudentResultDto result);
        Task<List<Admin>> GetStudentsByCourseIdAsync(int courseId);
        Task<List<StudentResultDto>> Getallresult(string studentID);





    }


}
