using AdmissionSystem.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace AdmissionSystem.DAL
{
    public class StudentRepository
    {
        private const string SQL_INSERT = @"insert into Student(FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId)
                                values(@FirstName, @LastName, @BirthDate, @Phone, @Email, @HasDebt, @Level, @ClassId)
                                select SCOPE_IDENTITY()";

        private const string SQL_GET_BY_ID = @"select StudentId, FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId
                                from Student
                                where StudentId = @StudentId";

        private const string SQL_UPDATE = @"update Teacher set
                                              FirstName = @FirstName, 
                                              LastName  = @LastName, 
                                              BirthDate  = @BirthDate,
                                              HasDebt  = @HasDebt,
                                              Level  = @Level,
                                              Phone  = @Phone,
                                              Email  = @Email,
                                              ClassId  = @ClassId
                                            where StudentId = @StudentId";

        private const string SQL_DELETE = @"delete from Student where StudentId = @StudentId";

        private readonly string _connStr;

        public StudentRepository(string connStr)
        {
            _connStr = connStr;
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();

            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"select StudentId, FirstName, LastName, BirthDate, HasDebt, Level, Phone, Email, ClassId
                                from Student";
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var student = new Student();
                student.StudentId = rdr.GetInt32(rdr.GetOrdinal("StudentId"));
                student.FirstName = rdr.GetString("FirstName");
                student.LastName = rdr.GetString("LastName");
                student.BirthDate = rdr.GetDateTime("BirthDate");
                student.HasDebt = rdr.GetBoolean("HasDebt");
                student.Level = rdr.GetInt32("Level");
                student.Phone = rdr.GetString("Phone");
                student.Email = rdr.GetString("Email");
                student.ClassId = rdr.GetInt32("ClassId");
                //teacher.Image = rdr.GetByte("Image");

                students.Add(student);
            }

            return students;
        }

        public IEnumerable<Student> Filter(StudentsFilterViewModel studentsFilterViewModel)
        {
            using var connection = new SqlConnection(_connStr);

            if(studentsFilterViewModel.BirthDate.Year <= 1901 || studentsFilterViewModel.BirthDate.Year >= 9998)
                studentsFilterViewModel.BirthDate = DateTime.Parse("1900-01-01T00:00:00");

            if( studentsFilterViewModel.PageNumber < 1 ) studentsFilterViewModel.PageNumber = 1;

            var parameters = new
            {
                FirstName = studentsFilterViewModel.FirstName,
                LastName = studentsFilterViewModel.LastName,
                BirthDate = studentsFilterViewModel.BirthDate,
                SortOrder = studentsFilterViewModel.SortDesc ? "DESC" : "ASC",
                SortColumn = studentsFilterViewModel.SortColumn,
                PageSize = studentsFilterViewModel.PageSize,
                PageNumber = studentsFilterViewModel.PageNumber,
            };

            var sql = "FilterStudents";
            var students = connection.Query<Student>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return students;
        }


        public string ExportAsXML( StudentsFilterViewModel studentsFilterViewModel )
        {
            string xml = null;

            using ( SqlConnection connection = new SqlConnection(_connStr) )
            {
                SqlCommand command = new SqlCommand("FilteredStudentsXml", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", studentsFilterViewModel.FirstName);
                command.Parameters.AddWithValue("@LastName", studentsFilterViewModel.LastName);
                command.Parameters.AddWithValue("@BirthDate", studentsFilterViewModel.BirthDate);
                command.Parameters.AddWithValue("@SortColumn", studentsFilterViewModel.SortColumn);
                command.Parameters.AddWithValue("@SortOrder", studentsFilterViewModel.SortDesc ? "DESC" : "ASC");
                command.Parameters.AddWithValue("@PageSize", studentsFilterViewModel.PageSize);
                command.Parameters.AddWithValue("@PageNumber", studentsFilterViewModel.PageNumber);

                connection.Open();
                xml = ( string )command.ExecuteScalar();
            }

            return xml;
        }

        public string ExportAsJSON( StudentsFilterViewModel studentsFilterViewModel )
        {
            string json = null;

            using ( SqlConnection connection = new SqlConnection(_connStr) )
            {
                SqlCommand command = new SqlCommand("FilteredStudentsJson", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", studentsFilterViewModel.FirstName);
                command.Parameters.AddWithValue("@LastName", studentsFilterViewModel.LastName);
                command.Parameters.AddWithValue("@BirthDate", studentsFilterViewModel.BirthDate);
                command.Parameters.AddWithValue("@SortColumn", studentsFilterViewModel.SortColumn);
                command.Parameters.AddWithValue("@SortOrder", studentsFilterViewModel.SortDesc ? "DESC" : "ASC");
                command.Parameters.AddWithValue("@PageSize", studentsFilterViewModel.PageSize);
                command.Parameters.AddWithValue("@PageNumber", studentsFilterViewModel.PageNumber);

                connection.Open();
                json = ( string )command.ExecuteScalar();
            }

            return json;
        }


        public Student GetStudentById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_GET_BY_ID;
            cmd.Parameters.AddWithValue("@StudentId", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                //return MapReaderToEmployee(rdr);
                var student = new Student();
                student.StudentId = rdr.GetInt32(rdr.GetOrdinal("StudentId"));
                student.FirstName = rdr.GetString("FirstName");
                student.LastName = rdr.GetString("LastName");
                student.BirthDate = rdr.GetDateTime("BirthDate");
                student.HasDebt = rdr.GetBoolean("HasDebt");
                student.Level = rdr.GetInt32("Level");
                student.Phone = rdr.GetString("Phone");
                student.Email = rdr.GetString("Email");
                student.ClassId = rdr.GetInt32("ClassId");
                //teacher.Image = rdr.GetByte("Image");
                return student;
            }

            return null;
        }

        public int Insert(Student student)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_INSERT;

            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@BirthDate", student.BirthDate);
            cmd.Parameters.AddWithValue("@HasDebt", student.HasDebt);
            cmd.Parameters.AddWithValue("@Level", student.Level);
            cmd.Parameters.AddWithValue("@Phone", student.Phone);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@ClassId", student.ClassId);
            //cmd.Parameters.AddWithValue("@Image", teacher.Image);

            conn.Open();
            var id = (decimal)cmd.ExecuteScalar();

            student.StudentId = (int)id;

            return (int)id;
        }

        public void Update(Student student)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_UPDATE;

            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@BirthDate", student.BirthDate);
            cmd.Parameters.AddWithValue("@HasDebt", student.HasDebt);
            cmd.Parameters.AddWithValue("@Level", student.Level);
            cmd.Parameters.AddWithValue("@Phone", student.Phone);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            //cmd.Parameters.AddWithValue("@Image", teacher.Image);
            cmd.Parameters.AddWithValue("@StudentId", student.StudentId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public bool BatchInsert( List<Student> students )
        {
            try
            {
                foreach ( var student in students )
                {
                    Insert(student);
                }
                return true;
            }
            catch
            {

                return false;
            }
        }

        public void Delete( int Id )
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_DELETE;
            cmd.Parameters.AddWithValue("@StudentId", Id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
