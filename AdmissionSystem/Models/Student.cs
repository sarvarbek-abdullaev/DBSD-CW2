using System;

namespace AdmissionSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int Level { get; set; }
        public bool HasDebt { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
    }
}
