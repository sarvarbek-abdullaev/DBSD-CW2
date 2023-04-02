using AdmissionSystem.Models;
using Dapper;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AdmissionSystem.DAL
{
    //public class CourseRepository : ICourseRepository
    //{
    //    private const string SQL_INSERT = @"insert into Class(Name, Description, TeacherId)
    //                            values(@Name, @Description, @TeacherId)
    //                            select SCOPE_IDENTITY()";

    //    private const string SQL_GET_BY_ID = @"select ClassId, Name, Description, TeacherId
    //                            from Class
    //                            where ClassId = @ClassId";

    //    private string ConnStr;

    //    public CourseRepository(string connStr)
    //    {
    //        ConnStr = connStr;
    //    }

    //    public List<Class> GetAll()
    //    {
    //        var employees = new List<Class>();

    //        using var conn = new SqlConnection(ConnStr);
    //        using var cmd = conn.CreateCommand();
    //        cmd.CommandText = @"select ClassId, Name, Description, TeacherId
    //                            from Class";
    //        conn.Open();
    //        using var rdr = cmd.ExecuteReader();
    //        while (rdr.Read())
    //        {
    //            var cs = new Class();
    //            cs.Id = rdr.GetInt32(rdr.GetOrdinal("ClassId"));
    //            cs.Name = rdr.GetString("Name");
    //            cs.Description = rdr.GetString("Description");
    //            cs.TeacherID = rdr.GetInt32("TeacherId");

    //            employees.Add(cs);
    //        }

    //        return employees;
    //    }

    //    public Class GetClassById(int id)
    //    {
    //        using var conn = new SqlConnection(ConnStr);
    //        using var cmd = conn.CreateCommand();
    //        cmd.CommandText = SQL_GET_BY_ID;
    //        cmd.Parameters.AddWithValue("@ClassId", id);

    //        conn.Open();
    //        using var rdr = cmd.ExecuteReader();
    //        if (rdr.Read())
    //        {
    //            //return MapReaderToEmployee(rdr);
    //            var cs = new Class();
    //            cs.Id = rdr.GetInt32(rdr.GetOrdinal("ClassId"));
    //            cs.Name = rdr.GetString("Name");
    //            cs.Description = rdr.GetString("Description");
    //            cs.TeacherID = rdr.GetInt32("TeacherId");

    //            return cs;
    //        }

    //        return null;
    //    }

    //    public int Insert(Class @class)
    //    {
    //        using var conn = new SqlConnection(ConnStr);
    //        using var cmd = conn.CreateCommand();
    //        cmd.CommandText = SQL_INSERT;


    //        cmd.Parameters.AddWithValue("@Name", @class.Name);
    //        cmd.Parameters.AddWithValue("@Description", @class.Description);
    //        cmd.Parameters.AddWithValue("@TeacherId", @class.TeacherID);

    //        conn.Open();
    //        var id = (decimal)cmd.ExecuteScalar();

    //        @class.TeacherID = (int)id;

    //        return (int)id;
    //    }
    //}
}
