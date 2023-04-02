using AdmissionSystem.Models;
using System.Collections.Generic;

namespace AdmissionSystem.DAL
{
    public interface IStudentRepository
    {
        List<Student> GetAll();
        int Insert(Student student);
        void Update(Student student);
        void Delete(int Id);
        Student GetStudentById(int Id);
        bool BatchInsert( List<Student> students );
    }
}
