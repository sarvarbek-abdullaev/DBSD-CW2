using AdmissionSystem.Models;
using System;
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

        List<Teacher> Filter(
            string firstname, string secondname,
            out int totalRows,
            int page = 1, int pageSize = 10,
            string sortColumn = "TeacherId", bool sortDesc = false);
    }
}
