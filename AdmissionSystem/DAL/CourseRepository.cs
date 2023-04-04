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
    public class CourseRepository : ICourseRepository
    {
        private const string SQL_INSERT = @"insert into Course(Name, Description)
                                values(@Name, @Description)
                                select SCOPE_IDENTITY()";

        private const string SQL_GET_BY_ID = @"select CourseId, Name, Description
                                from Course
                                where CourseId = @CourseId";

        private string ConnStr;

        public CourseRepository(string connStr)
        {
            ConnStr = connStr;
        }

        public List<Course> GetAll()
        {
            var courses = new List<Course>();

            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"select CourseId, Name, Description
                                from Course";
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var course = new Course();
                course.CourseId = rdr.GetInt32(rdr.GetOrdinal("CourseId"));
                course.Name = rdr.GetString("Name");
                course.Description = rdr.GetString("Description");

                courses.Add(course);
            }

            return courses;
        }

        public Course GetCourseById(int id)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_GET_BY_ID;
            cmd.Parameters.AddWithValue("@courseId", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                var course = new Course();
                course.CourseId = rdr.GetInt32(rdr.GetOrdinal("CourseId"));
                course.Name = rdr.GetString("Name");
                course.Description = rdr.GetString("Description");

                return course;
            }

            return null;
        }

        public int Insert(Course course)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_INSERT;


            cmd.Parameters.AddWithValue("@Name", course.Name);
            cmd.Parameters.AddWithValue("@Description", course.Description);

            conn.Open();
            var id = (decimal)cmd.ExecuteScalar();

            return (int)id;
        }
    }
}
