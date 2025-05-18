using CUG_ONLINE_COURSES.Data;
using CUG_ONLINE_COURSES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CUG_ONLINE_COURSES.Services.RolesServices
{
    public class RoleService : IRoleService
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;  // Add this line for DbContext
        private readonly string _storagePath;

        public RoleService(IConfiguration configuration,RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _storagePath = configuration["FileStoragePath"] ?? "wwwroot/uploads/library/";
        }

        public async Task EnsureRolesExistAsync()
        {
            foreach (var role in AuthenticationInitalData.AppRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }
        public async Task<bool> SaveUserRoles(UserWithRoles user)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.UserId);
            if (userToUpdate != null)
            {
                var userWithOldRoles = await GetUsersWithRolesByUserAsync(userToUpdate);
                if (!string.IsNullOrEmpty(userWithOldRoles.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(userToUpdate, userWithOldRoles.RoleName);
                }

                if (!string.IsNullOrEmpty(user.RoleName))
                {
                    await _userManager.AddToRoleAsync(userToUpdate, user.RoleName);
                }
                return true;
            }
            return false;
        }
        public async Task<List<UserWithRoles>> GetUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();
            var userWithRolesList = new List<UserWithRoles>();

            foreach (var user in users)
            {
                var userWithRoles = new UserWithRoles
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    userWithRoles.RoleName = roles.FirstOrDefault();
                }
                userWithRolesList.Add(userWithRoles);
            }

            return userWithRolesList;
        }
        public async Task<UserWithRoles> GetUsersWithRolesByUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return new UserWithRoles
            {
                UserId = user.Id,
                UserName = user.UserName,
                RoleName = roles.FirstOrDefault()
            };
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            try
            {
                var Admin = _userManager.Users.ToList();
                var AdminList = new List<Admin>();

                foreach (var user in Admin)
                {
                    var admin = new Admin
                    {
                        AdminID = user.Id,
                        Email = user.UserName,
                        Name = user.FullName,
                        Phone = user.PhoneNumber,
                        Password = user.PasswordHash,
                        createdOn = user.createdOn,
                        Status = user.status,
                    };
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Count > 0)
                    {
                        admin.Role = roles.FirstOrDefault();
                    }
                    AdminList.Add(admin);
                }
                return AdminList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<Admin>> GetAllStaff()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userWithRolesList = new List<Admin>();

                foreach (var user in users)
                {
                    var staff = new Admin
                    {
                        AdminID = user.Id,
                        Email = user.UserName,
                        Name = user.FullName,
                    };
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Count > 0)
                    {
                        staff.Role = roles.FirstOrDefault();
                    }
                    // Check if the user is a "Student" and skip adding them to the list
                    if (!roles.Contains("Student"))  // Assuming "Student" is the role you want to exclude
                    {
                        userWithRolesList.Add(staff);  // Add to list only if not a student
                    }

                }
                //return null;
                return userWithRolesList;
            }
            catch (Exception ex)
            {
                return new List<Admin>();
            }
        }

        public async Task<bool> AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                var response = await _context.SaveChangesAsync();
                if (response == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex) {
                return false;

            }
        }
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {

                // return await _context.Courses.ToListAsync(); // Return all courses from the database
                var coursesWithLecturers = await _context.Courses

        .Join(
            _userManager.Users, // Assuming Admins is the table holding the lecturer info
            course => course.AssignedLecturer, // The foreign key in the course table (e.g., AdminID)
            lecturer => lecturer.Id, // The primary key in the Admins table
            (course, lecturer) => new Course
            {
                Id = course.Id,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Faculty = course.Faculty,
                AssignedLecturer = lecturer.Id,
                AssignedLecturerName = lecturer.FullName,
                CourseDuration = course.CourseDuration,

                // Lecturer Name from Admins table
            })
        .ToListAsync();

                return coursesWithLecturers;

            }
            catch
            {
                return new List<Course>();
            }
        }


        public async Task<List<Course>> GetAllFacultyCoursesAsync(string facultyID)
        {
            try
            {
                var coursesWithLecturers = await _context.Courses
    .Join(
        _userManager.Users, // Assuming Users table holds lecturer info
        course => course.AssignedLecturer, // Foreign key in Courses table
        lecturer => lecturer.Id, // Primary key in Users table
        (course, lecturer) => new Course
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Faculty = course.Faculty,
            AssignedLecturer = lecturer.Id,
            AssignedLecturerName = lecturer.FullName, // Lecturer Name from Users table
        })
    .Where(course => course.Faculty.Trim() == facultyID.Trim()) // Replace "YourFacultyName" with your filter value
    .ToListAsync();

                return coursesWithLecturers;
            }
            catch
            {
                return new List<Course>();
            }
        }
        public async Task<List<LibraryItem>> GetLibraryItemsAsync()
        {
            return await _context.LibraryItems.ToListAsync();
        }

        public async Task<bool> AddLibraryItemAsync(LibraryItem item)
        {
              _context.LibraryItems.Add(item);
           var response= await _context.SaveChangesAsync();
            if (response == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<Dashboard_Data> DashboardData()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync(); // Fetch all users from the database
                var courses = await _context.Courses.CountAsync(); // Fetch total courses count

                int totalStudents = 0;
                int totalStaff = 0;

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user); // Fetch user roles
                    if (roles.Contains("Student"))
                    {
                        totalStudents++; // Count students
                    }
                    else
                    {
                        totalStaff++; // Count staff (excluding students)
                    }
                }

                return new Dashboard_Data
                {
                    TotalUser = users.Count,
                    TotalStudent = totalStudents,
                    TotalStaff = totalStaff,
                    TotalActiveCourse = courses
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching dashboard data: {ex.Message}");
                return new Dashboard_Data
                {
                    TotalUser = 0,
                    TotalStudent = 0,
                    TotalStaff = 0,
                    TotalActiveCourse = 0
                };
            }
        }


        public async Task<List<staffDashboad>> DashboardDataStaff(string userID)
        {
            try
            {
                var staffDashboardList = await _context.Courses
                    .Where(course => course.AssignedLecturer == userID)
                    .Select(course => new
                    {
                        Course = course,
                        TotalStudents = _context.StudentCourses
                            .Where(sc => sc.CourseId == course.Id)
                            .Count()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                // Map the anonymous result to the staffDashboad model
                var dashboardData = staffDashboardList.Select(item => new staffDashboad
                {
                    CourseCode=item.Course.CourseCode,
                    CourseName = item.Course.CourseName,
                    TotalStudents = item.TotalStudents
                }).ToList();

                return dashboardData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching staff dashboard data: {ex.Message}");
                return new List<staffDashboad>();
            }
        }

        public async Task<bool> RegisterStudentForCourse(string studentId, int courseId)
        {
            try
            {
                // Check if the student is already registered
                bool alreadyRegistered = await _context.StudentCourses
                    .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                if (alreadyRegistered)
                {
                    return false; // Student is already registered
                }

                var newRegistration = new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = courseId
                };

                _context.StudentCourses.Add(newRegistration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false; // Registration failed
            }
        }


        public async Task<List<Course>> GetStudentRegisteredCourses(string studentId)
        {
            try
            {
                var registeredCourses = await _context.StudentCourses
                    .Where(sc => sc.StudentId == studentId)
                    .Select(sc => new Course
                    {
                        Id = sc.Course.Id,
                        CourseCode = sc.Course.CourseCode,
                        CourseName = sc.Course.CourseName,
                        Faculty=sc.Course.Faculty,
                        CourseDuration = sc.Course.CourseDuration,
                       
                    })
                    .ToListAsync();

                return registeredCourses;
            }
            catch (Exception)
            {
                return new List<Course>(); // Return empty list on error
            }
        }




        public async Task<List<StudentDto>> GetStudentsRegisteredForCourse(string courseCode)
        {
            return await _context.StudentCourses
                .Where(sc => sc.Course.CourseCode == courseCode)
                .Include(sc => sc.Student)
                .Select(sc => new StudentDto
                {
                    Id = sc.Student.Id,
                    Email=sc.Student.Email,
                    FullName = sc.Student.FullName
                })
                .ToListAsync();
        }




        public async Task<Course> GetCourseByIdAsync(int courseId) {
            try
            {
                // Fetch the course from the database by courseId
                var course = await _context.Courses
                    .Where(c => c.Id == courseId)
                    .FirstOrDefaultAsync();

                return course; // Return the found course or null if not found
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Console.WriteLine($"Error occurred while retrieving course: {ex.Message}");
                return null;
            }

        }
        public async Task<bool> EditCourse(Course course) {

            try
            {
                // Find the course to update by its ID
                var existingCourse = await _context.Courses
                    .Where(c => c.Id == course.Id)
                    .FirstOrDefaultAsync();

                if (existingCourse != null)
                {
                    // Update course properties
                    existingCourse.CourseCode = course.CourseCode;
                    existingCourse.CourseName = course.CourseName;
                    existingCourse.Faculty = course.Faculty;
                    existingCourse.AssignedLecturer = course.AssignedLecturer;
                    existingCourse.AssignedLecturerName = course.AssignedLecturerName;

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false; // Course not found, so return false
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error occurred while editing course: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Admin>> GetStudentsByCourseIdAsync(int courseId) {
            try
            {
                // Get all students registered for the given courseId
                var studentsInCourse = await _context.StudentCourses
                    .Where(sc => sc.CourseId == courseId)
                    .Join(_userManager.Users,
                        sc => sc.StudentId,
                        student => student.Id,
                        (sc, student) => new Admin
                        {
                            AdminID = student.Id,
                            Email = student.UserName,
                            Name = student.FullName,
                            Phone = student.PhoneNumber,
                            createdOn = student.createdOn,
                            Status = student.status
                        })
                    .ToListAsync();

                return studentsInCourse;
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error occurred while retrieving students for course {courseId}: {ex.Message}");
                return new List<Admin>(); // Return an empty list if an error occurs
            }
        }

        public async Task<bool> AddStudentResultAsync(StudentResultDto resultDto)
        {
            try
            {
                // Check if result already exists
                var existingResult = await _context.StudentResult
                    .FirstOrDefaultAsync(r => r.studentID == resultDto.studentID
                                           && r.CourseID == resultDto.CourseID);

                if (existingResult != null)
                {
                    // Update existing result
                    existingResult.Score = resultDto.Score;
                    existingResult.Grade = resultDto.Grade;

                    _context.StudentResult.Update(existingResult);
                }
                else
                {
                    // Create new result
                  

                    await _context.StudentResult.AddAsync(resultDto);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving student result: {ex.Message}");
                return false;
            }
        }

        public async Task<List<StudentResultDto>> Getallresult(string studentID)
        {
            try
            {
                var results = await _context.StudentResult
                    .Where(r => r.studentID == studentID)
                    .Select(r => new StudentResultDto
                    {
                        Id = r.Id,
                        studentID = r.studentID,
                        CourseID = r.CourseID,
                        Score = r.Score,
                        Grade = r.Grade
                    })
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving student results: {ex.Message}");
                return new List<StudentResultDto>();
            }
        }



    }
}
