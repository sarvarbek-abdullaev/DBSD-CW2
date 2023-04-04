namespace AdmissionSystem.Models
{
    public class Teacher : Person
    {
        public int? TeacherId { get; set; }
        public decimal Salary { get; set; }
        public bool IsMarried { get; set; }
    }
}
