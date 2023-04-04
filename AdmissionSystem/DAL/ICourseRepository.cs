using AdmissionSystem.Models;
using System.Collections.Generic;

namespace AdmissionSystem.DAL
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        int Insert(Course course);
        Course GetCourseById(int Id);
    }
}
