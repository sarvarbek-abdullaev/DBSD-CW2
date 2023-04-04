using AdmissionSystem.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace AdmissionSystem.DAL
{
    public class StudentRepository
    {
        private const string SQL_INSERT = @"insert into Student(FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId, Image)
                                values(@FirstName, @LastName, @BirthDate, @Phone, @Email, @HasDebt, @Level, @ClassId, @Image)
                                select SCOPE_IDENTITY()";

        private const string SQL_GET_BY_ID = @"select StudentId, FirstName, LastName, BirthDate, Phone, Email, HasDebt, Level, ClassId, Image
                                from Student
                                where StudentId = @StudentId";

        private const string SQL_UPDATE = @"update Student set
                                              FirstName = @FirstName, 
                                              LastName  = @LastName, 
                                              BirthDate  = @BirthDate,
                                              HasDebt  = @HasDebt,
                                              Level  = @Level,
                                              Phone  = @Phone,
                                              Email  = @Email,
                                              ClassId  = @ClassId,
                                              Image  = @Image
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
            cmd.CommandText = @"select StudentId, FirstName, LastName, BirthDate, HasDebt, Level, Phone, Email, ClassId, Image
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

                if (!rdr.IsDBNull(rdr.GetOrdinal("Image")))
                    student.Image = (byte[])rdr["Image"];

                students.Add(student);
            }

            return students;
        }

        public IEnumerable<FilteredStudentRow> Filter(StudentsFilterViewModel studentsFilterViewModel)
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
            var filteredStudentRows = connection.Query<FilteredStudentRow>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return filteredStudentRows;
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

        public void ImportJSON(string json)
        {
            using SqlConnection connection = new SqlConnection(_connStr);

            SqlCommand command = new SqlCommand("ImportStudentsJson", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@jsonString", json);

            connection.Open();

            command.ExecuteScalar();
        }

        public void ImportXML( string xml )
        {
            using SqlConnection connection = new SqlConnection(_connStr);

            SqlCommand command = new SqlCommand("ImportStudentsXml", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@xmlString", xml);

            connection.Open();

            command.ExecuteScalar();
        }

        public void ImportBulkStudents( IEnumerable<Student> students )
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("FirstName", typeof(string));
            dataTable.Columns.Add("LastName", typeof(string));
            dataTable.Columns.Add("BirthDate", typeof(DateTime));
            dataTable.Columns.Add("Phone", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("HasDebt", typeof(string));
            dataTable.Columns.Add("Level", typeof(int));
            dataTable.Columns.Add("ClassId", typeof(int));

            foreach ( var s in students )
            {
                dataTable.Rows.Add(
                        s.FirstName, 
                        s.LastName, 
                        s.BirthDate,
                        s.Phone, 
                        s.Email,
                        s.HasDebt,
                        s.Level,
                        s.ClassId
                    );
            }

            using var conn = new SqlConnection(_connStr);
            conn.Execute("ImportBulkStudents",
                new { Students = dataTable },
                commandType: CommandType.StoredProcedure
            );
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

                if (!rdr.IsDBNull(rdr.GetOrdinal("Image")))
                    student.Image = (byte[])rdr["Image"];

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
            cmd.Parameters.AddWithValue("@Image", student.Image ?? SqlBinary.Null);

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
            cmd.Parameters.AddWithValue("@Image", student.Image ?? SqlBinary.Null);
            cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
            cmd.Parameters.AddWithValue("@ClassId", student.ClassId);

            conn.Open();
            cmd.ExecuteNonQuery();
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
