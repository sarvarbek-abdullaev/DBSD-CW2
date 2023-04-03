using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdmissionSystem.Models
{
    public class StudentsFilterViewModel
    {
        public IEnumerable<Student> Students = new List<Student>();
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "StudentId";
        public bool SortDesc { get; set; } = false;
    }
}
