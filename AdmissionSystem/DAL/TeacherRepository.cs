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
    public class TeacherRepository : ITeacherRepository
    {
        private const string SQL_INSERT = @"insert into Teacher(FirstName, LastName, BirthDate, IsMarried, Salary, Phone, Email)
                                values(@FirstName, @LastName, @BirthDate, @IsMarried, @Salary, @Phone, @Email)
                                select SCOPE_IDENTITY()";

        private const string SQL_GET_BY_ID = @"select TeacherId, FirstName, LastName, BirthDate, IsMarried, Salary, Phone, Email
                                from Teacher
                                where TeacherId = @TeacherId";

        private const string SQL_UPDATE = @"update Teacher set
                                              FirstName = @FirstName, 
                                              LastName  = @LastName, 
                                              BirthDate  = @BirthDate,
                                              IsMarried  = @IsMarried,
                                              Salary  = @Salary,
                                              Phone  = @Phone,
                                              Email  = @Email
                                            where TeacherId = @TeacherId";
        private const string SQL_DELETE = @"delete from Teacher where TeacherId = @TeacherId";

        private const string SQL_FILTER = @"select TeacherId, FirstName, Lastname, BirthDate,
	                                            count(*) over() TotalRowsCount
                                            from Teacher
                                            where FirstName like coalesce(@FirstName, '') + '%'
	                                            and Lastname like coalesce(@Lastname, '') + '%'
                                            order by {0}
                                            offset @OffsetRows rows
                                            fetch next @PageSize rows only";

        private string ConnStr;

        public TeacherRepository(string connStr)
        {
            ConnStr = connStr;
        }

        public void Delete(int Id)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_DELETE;
            cmd.Parameters.AddWithValue("@TeacherId", Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Teacher> Filter(string firstname, string secondname, out int totalRows, int page = 1, int pageSize = 10, string sortColumn = "TeacherId", bool sortDesc = false)
        {
            //using var conn = new SqlConnection(ConnStr);
            //var teachers = conn.Query<Teacher>(
            //    "TeacherFilter",
            //    commandType: System.Data.CommandType.StoredProcedure,
            //    param: new { FirstName = firstname, LastName = secondname, OffsetRows = (page - 1) * pageSize, PageSize = pageSize }
            //    );

            //totalRows = teachers.FirstOrDefault()?.TotalRowsCount ?? 0;

            //return teachers.AsList();

            if (page <= 0)
                page = 1;

            var sort = "TeacherId";
            if ("TeacherId".Equals(sortColumn))
                sort = "TeacherId";
            else if ("FirstName".Equals(sortColumn))
                sort = "FirstName";

            if (sortDesc)
                sort += " DESC ";

            string sql = string.Format(SQL_FILTER, sort);

            using var conn = new SqlConnection(ConnStr);
            var teachers = conn.Query<Teacher>(
                sql,
                new
                {
                    FirstName = firstname,
                    Lastname = secondname,
                    OffsetRows = (page - 1) * pageSize,
                    PageSize = pageSize
                }).AsList();

            totalRows = teachers.FirstOrDefault()?.TotalRowsCount ?? 0;

            return teachers;

        }

        public List<Teacher> GetAll()
        {
            var employees = new List<Teacher>();

            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"select TeacherId, FirstName, LastName, BirthDate, IsMarried, Salary, Phone, Email
                                from Teacher";
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var teacher = new Teacher();
                teacher.TeacherId = rdr.GetInt32(rdr.GetOrdinal("TeacherId"));
                teacher.FirstName = rdr.GetString("FirstName");
                teacher.LastName = rdr.GetString("LastName");
                teacher.BirthDate = rdr.GetDateTime("BirthDate");
                teacher.IsMarried = rdr.GetBoolean("IsMarried");
                teacher.Salary = rdr.GetInt32("Salary");
                teacher.Phone = rdr.GetString("Phone");
                teacher.Email = rdr.GetString("Email");
                //teacher.Image = rdr.GetByte("Image");

                employees.Add(teacher);
            }

            return employees;
        }

        public Teacher GetTeacherById(int id)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_GET_BY_ID;
            cmd.Parameters.AddWithValue("@TeacherId", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                //return MapReaderToEmployee(rdr);
                var teacher = new Teacher();
                teacher.TeacherId = rdr.GetInt32(rdr.GetOrdinal("TeacherId"));
                teacher.FirstName = rdr.GetString("FirstName");
                teacher.LastName = rdr.GetString("LastName");
                teacher.BirthDate = rdr.GetDateTime("BirthDate");
                teacher.IsMarried = rdr.GetBoolean("IsMarried");
                teacher.Salary = rdr.GetInt32("Salary");
                teacher.Phone = rdr.GetString("Phone");
                teacher.Email = rdr.GetString("Email");
                return teacher;
            }

            return null;
        }

        public int Insert(Teacher teacher)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_INSERT;

            cmd.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            cmd.Parameters.AddWithValue("@LastName", teacher.LastName);
            cmd.Parameters.AddWithValue("@BirthDate", teacher.BirthDate);
            cmd.Parameters.AddWithValue("@IsMarried", teacher.IsMarried);
            cmd.Parameters.AddWithValue("@Salary", teacher.Salary);
            cmd.Parameters.AddWithValue("@Phone", teacher.Phone);
            cmd.Parameters.AddWithValue("@Email", teacher.Email);
            //cmd.Parameters.AddWithValue("@Image", teacher.Image);

            conn.Open();
            var id = (decimal)cmd.ExecuteScalar();

            teacher.TeacherId = (int)id;

            return (int)id;
        }

        public void Update(Teacher teacher)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_UPDATE;

            cmd.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            cmd.Parameters.AddWithValue("@LastName", teacher.LastName);
            cmd.Parameters.AddWithValue("@BirthDate", teacher.BirthDate);
            cmd.Parameters.AddWithValue("@IsMarried", teacher.IsMarried);
            cmd.Parameters.AddWithValue("@Salary", teacher.Salary);
            cmd.Parameters.AddWithValue("@Phone", teacher.Phone);
            cmd.Parameters.AddWithValue("@Email", teacher.Email);
            cmd.Parameters.AddWithValue("@TeacherId", teacher.TeacherId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
