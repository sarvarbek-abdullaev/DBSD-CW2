using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdmissionSystem.Models
{
    public class TeacherCreateViewModel
    {
        [DisplayName("ID")]
        public int? TeacherId { get; set; }
        [DisplayName("Name")]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        public string LastName { get; set; }
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Salary")]
        public decimal Salary { get; set; }
        [DisplayName("Married ?")]
        public bool IsMarried { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        [DisplayName("Image")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
