using AdmissionSystem.Models;
using System;
using System.Collections.Generic;

namespace AdmissionSystem.DAL
{
    public interface IClassRepository
    {
        List<Class> GetAll();
        int Insert(Class @class);
        Class GetClassById(int Id);

        //List<Class> Filter(
        //    string firstname, string secondname,
        //    out int totalRows,
        //    int page = 1, int pageSize = 10,
        //    string sortColumn = "TeacherId", bool sortDesc = false);
    }
}
