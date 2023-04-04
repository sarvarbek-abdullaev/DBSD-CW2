using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdmissionSystem.Models
{
    public class StudentCreateViewModel
    {
        [DisplayName("ID")]
        public int? StudentId { get; set; }
        [DisplayName("Name")]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        public string LastName { get; set; }
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Level")]
        public int Level { get; set; }
        [DisplayName("Has Debt ?")]
        public bool HasDebt { get; set; }
        [DisplayName("Class")]
        public int ClassId { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        [DisplayName("Image")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public Class Class { get; set; }
    }
}
