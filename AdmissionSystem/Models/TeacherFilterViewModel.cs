using Microsoft.Graph;
using System.Collections.Generic;
using System;

namespace AdmissionSystem.Models
{
    public class TeacherFilterViewModel
    {
        public IList<Teacher> Teachers;
        public int TotalRows { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "TeacherId";
        public bool SortDesc { get; set; } = false;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
