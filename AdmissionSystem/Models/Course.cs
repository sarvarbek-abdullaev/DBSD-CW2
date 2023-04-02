using System.Collections.Generic;

namespace AdmissionSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalRowsCount { get; set; } = 1;
    }
}
