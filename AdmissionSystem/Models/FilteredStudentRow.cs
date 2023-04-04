using System;

namespace AdmissionSystem.Models
{
    public class FilteredStudentRow
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int Level { get; set; }
        public bool HasDebt { get; set; }
        public string ClassName { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
    }
}
