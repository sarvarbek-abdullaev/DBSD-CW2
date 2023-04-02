using AdmissionSystem.Models;
using System.Collections.Generic;

namespace AdmissionSystem.DAL
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        int Insert(Course course);
        Course GetCourseById(int Id);

        //List<Course> Filter(
        //    string firstname, string secondname,
        //    out int totalRows,
        //    int page = 1, int pageSize = 10,
        //    string sortColumn = "TeacherId", bool sortDesc = false);
    }
}
