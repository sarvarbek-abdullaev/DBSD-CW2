using System;

namespace AdmissionSystem.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Image { get; set; }
        public int TotalRowsCount { get; set; } = 1;
    }
}
