using System;
using System.Collections.Generic;

namespace AdmissionSystem.Models
{
    public class StudentsFilterViewModel
    {
        public IEnumerable<FilteredStudentRow> FilteredStudentRows = new List<FilteredStudentRow>();
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "StudentId";
        public bool SortDesc { get; set; } = false;
    }
}
