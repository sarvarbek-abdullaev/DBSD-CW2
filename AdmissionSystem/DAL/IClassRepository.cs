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
    }
}
