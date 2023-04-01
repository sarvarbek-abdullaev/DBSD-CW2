using System;

namespace AdmissionSystem.Models
{
    public class Student : Person
    {
        public int StudentId { get; set; }
        public int Level { get; set; }
        public bool HasDebt { get; set; }
        public int ClassId { get; set; }
    }
}
