using System.Collections.Generic;

namespace AdmissionSystem.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
    }
}
