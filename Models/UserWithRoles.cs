using CUG_ONLINE_COURSES.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CUG_ONLINE_COURSES.Models
{
    public class UserWithRoles
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool Error { get; set; }
        public bool Saved { get; set; }
        public bool Saving { get; set; }
    }

    public class Admin
    {


        public string AdminID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

        public DateTime? createdOn { get; set; }

    }
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "CourseCode")]
        public string CourseCode { get; set; } = "";
        [Required]
        [Display(Name = "CourseName")]
        public string CourseName { get; set; } = "";
        [Required]
        [Display(Name = "Faculty")]
        public string Faculty { get; set; } = "";
        [Required]
        [Display(Name = "AssignedLecturer")]
        public string AssignedLecturer { get; set; } = "";

        public string AssignedLecturerName { get; set; } = "";

        public bool IsRegistered { get; set; }

        [Required]
        [Display(Name = "CourseDuration")]
        public string CourseDuration { get; set; }
        


    }
    public class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., "Textbook"

        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        // Academic Year if applicable (e.g., 2022/2023 for past questions)
        public string AcademicYear { get; set; } = string.Empty;

        public string Semester { get; set; } = string.Empty; // Example: "First Semester"
        public string CourseCode { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        // Tags for searching (e.g., Math, Semester 1)
        public string Tags { get; set; } = string.Empty;


        // Optional: A more detailed description of the item
        public string Description { get; set; } = string.Empty;
    }

    public class Dashboard_Data
    {
        public int TotalUser { get; set; }
        public int TotalStudent { get; set; }
        public int TotalStaff { get; set; }
        public int TotalActiveCourse { get; set; }

    }
    public class staffDashboad
    {
        public string CourseCode { get; set;}
        public string CourseName { get; set; }
        public int TotalStudents { get; set; }

    }
    public class StudentCourse
    {
        public int Id { get; set; }
        public string StudentId { get; set; }  // Foreign key (UserID of Student)
        public int CourseId { get; set; }  // Foreign key (Course ID)

        public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;

        public ApplicationUser Student { get; set; }  // Navigation property
        public Course Course { get; set; }  // Navigation property
    }
    public class StudentDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
    }

    public class StudentResultDto
    {
      
        public int Id { get; set; }
        public string studentID { get; set; }

        public string CourseID { get; set; }

        public int Score { get; set; }

        public string Grade { get; set;}


    }
}
