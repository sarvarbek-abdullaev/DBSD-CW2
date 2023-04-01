using AdmissionSystem.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AdmissionSystem.DAL
{
    public class StudentRepository : IStudentRepository
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

        private const string SQL_FILTER = @"select StudentId, FirstName, Lastname,
	                                            count(*) over() TotalRowsCount
                                            from Teacher
                                            where FirstName like coalesce(@FirstName, '') + '%'
	                                            and Lastname like coalesce(@Lastname, '') + '%'
                                            order by {0}
                                            offset @OffsetRows rows
                                            fetch next @PageSize rows only";

        private string ConnStr;

        public StudentRepository(string connStr)
        {
            ConnStr = connStr;
        }

        public void Delete(int Id)
        {
            using var conn = new SqlConnection(ConnStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SQL_DELETE;
            cmd.Parameters.AddWithValue("@StudentId", Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();

            using var conn = new SqlConnection(ConnStr);
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

        public Student GetStudentById(int id)
        {
            using var conn = new SqlConnection(ConnStr);
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
            using var conn = new SqlConnection(ConnStr);
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
            using var conn = new SqlConnection(ConnStr);
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
    }
}
