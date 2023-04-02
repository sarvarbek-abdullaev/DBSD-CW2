using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AdmissionSystem.Models
{
    public class PersonCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
        public int TotalRowsCount { get; set; } = 1;
    }
}
