﻿using AdmissionSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AdmissionSystem.DAL
{
    public class TeacherRepository : ITeacherRepository
    {
        private const string SQL_INSERT = @"insert into Teacher(FirstName, LastName, BirthDate, IsMarried, Salary, Phone, Email)
                                values(@FirstName, @LastName, @BirthDate, @IsMarried, @Salary, @Phone, @Email)
                                select SCOPE_IDENTITY()";
        private const string SQL_GET_BY_ID = @"select EmployeeId, FirstName, LastName, BirthDate
                                from Employee
                                where EmployeeId = @EmployeeId";
        private const string SQL_UPDATE = @"update Employee set
                                              FirstName = @FirstName, 
                                              LastName  = @LastName, 
                                              BirthDate  = @BirthDate
                                            where EmployeeId = @EmployeeId";
        private const string SQL_DELETE = @"delete from Employee where EmployeeId = @EmployeeId";

        private string ConnStr;

        public TeacherRepository(string connStr)
        {
            ConnStr = connStr;
        }

        public void Delete(int Id)
        {
            throw new System.NotImplementedException();
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

        public Teacher GetTeacherById(int Id)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}
