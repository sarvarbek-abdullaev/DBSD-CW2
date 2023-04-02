using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System;
using Newtonsoft.Json;
using CsvHelper;
using System.Linq;

namespace AdmissionSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _repository;
        public StudentController(IStudentRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var students = _repository.GetAll();
            return View(students);
        }

        public ActionResult Details(int id)
        {
            var student = _repository.GetStudentById(id);
            return View(student);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
            try
            {
                int id = _repository.Insert(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var student = _repository.GetStudentById(id);

            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(int id, Student student)
        {
            try
            {
                student.StudentId = id;
                _repository.Update(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var student = _repository.GetStudentById(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _repository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Import( IFormFile importFile )
        {
            if ( importFile == null ) return RedirectToAction(nameof(Index));

            string extension = System.IO.Path.GetExtension(importFile.FileName);

            try
            {
                var students = new List<Student>();

                if ( extension == ".json" )
                {
                    using var stream = importFile.OpenReadStream();
                    using var reader = new StreamReader(stream);
                    students = System.Text.Json.JsonSerializer.Deserialize<List<Student>>(reader.ReadToEnd());
                }
                else if ( extension == ".xml" )
                {
                    using var stream = importFile.OpenReadStream();
                    using var reader = new StreamReader(stream);
                    var serializer = new XmlSerializer(typeof(List<Student>));
                    students = ( List<Student> )serializer.Deserialize(reader);

                }
                else if ( extension == ".csv" )
                {
                    using var stream = importFile.OpenReadStream();
                    using var reader = new StreamReader(stream);
                    using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    students = csvReader.GetRecords<Student>().ToList();
                }

                _repository.BatchInsert(students);
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult ExportJSON()
        {
            var students = _repository.GetAll();

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, students);
            writer.Flush();


            memory.Position = 0;
            if ( memory != Stream.Null )
                return File(memory, "application/json", $"Export_{DateTime.Now}.json");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult ExportXML()
        {
            var students = _repository.GetAll();

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var serializer = new XmlSerializer(typeof(List<Student>));
            serializer.Serialize(writer, students);
            writer.Flush();

            memory.Position = 0;
            if ( memory != Stream.Null )
                return File(memory, "application/xml", $"Export_{DateTime.Now}.xml");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult ExportCSV()
        {
            var students = _repository.GetAll();

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(students);
            writer.Flush();

            memory.Position = 0;
            if ( memory != Stream.Null )
                return File(memory, "text/csv", $"Export_{DateTime.Now}.csv");

            return RedirectToAction(nameof(Index));
        }
    }
}
