using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System;
using CsvHelper;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace AdmissionSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;
        public StudentController(StudentRepository repository)
        {
            _repository = repository;
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

        public ActionResult Index()
        {
            var students = _repository.GetAll();
            return View(students);
        }

        [HttpPost]
        public ActionResult Import( IFormFile importFile )
        {
            if ( importFile == null ) return RedirectToAction(nameof(Index));

            string extension = System.IO.Path.GetExtension(importFile.FileName);

                using var stream = importFile.OpenReadStream();

                using var reader = new StreamReader(stream);

                if ( extension == ".json" )
                {
                    _repository.ImportJSON(reader.ReadToEnd());
                }
                else if ( extension == ".xml" )
                {
                    _repository.ImportXML(reader.ReadToEnd());

                }
                else if ( extension == ".csv" )
                {
                    var students = new List<Student>();

                    var serializer = new CsvReader(reader, CultureInfo.InvariantCulture);
                    students = serializer.GetRecords<Student>().ToList<Student>();

                    _repository.ImportBulkStudents(students);
                }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Filter( StudentsFilterViewModel studentsFilterViewModel )
        {
            try
            {
                studentsFilterViewModel.FilteredStudentRows = _repository.Filter(studentsFilterViewModel);
                return View(studentsFilterViewModel);
            }
            catch
            {
                return RedirectToAction(nameof(Filter));
            }
        }

        [HttpPost]
        public ActionResult ExportCSV( StudentsFilterViewModel studentsFilterViewModel )
        {
            try
            {
                var students = _repository.Filter(studentsFilterViewModel);
                var memory = new MemoryStream();
                var writer = new StreamWriter(memory);
                var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(students);
                writer.Flush();

                memory.Position = 0;
                if ( memory != Stream.Null )
                    return File(memory, "text/csv", $"Students_{DateTime.Now}.csv");
            }
            catch {}

            return RedirectToAction(nameof(Filter));
        }

        [HttpPost]
        public ActionResult ExportJSON( StudentsFilterViewModel studentsFilterViewModel )
        {
            try
            {
                var json = _repository.ExportAsJSON(studentsFilterViewModel);

                if (!string.IsNullOrWhiteSpace(json)) 
                    return File(Encoding.UTF8.GetBytes(json), "application/json", $"Students_{DateTime.Now}.json");
            } catch {}
            
            return RedirectToAction(nameof(Filter));
        }

        [HttpPost]
        public ActionResult ExportXML( StudentsFilterViewModel studentsFilterViewModel )
        {
            try
            {
                var xml = _repository.ExportAsXML(studentsFilterViewModel);

                if (!string.IsNullOrWhiteSpace(xml))
                    return File(Encoding.UTF8.GetBytes(xml), "application/xml", $"Students_{DateTime.Now}.xml");

            } catch {}

            return RedirectToAction(nameof(Filter));
        }
    }
}
