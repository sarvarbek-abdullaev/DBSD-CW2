using AdmissionSystem.Models;
using System.Collections.Generic;

namespace AdmissionSystem.DAL
{
    public interface ITeacherRepository
    {
        List<Teacher> GetAll();
        int Insert(Teacher teacher);
        void Update(Teacher teacher);
        void Delete(int Id);
        Teacher GetTeacherById(int Id);
    }
}
