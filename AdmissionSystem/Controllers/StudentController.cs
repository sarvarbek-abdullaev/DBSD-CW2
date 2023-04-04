using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System;
using CsvHelper;
using System.Text;

namespace AdmissionSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;
        private readonly IClassRepository _class_repository;

        public StudentController(StudentRepository repository, IClassRepository class_repository)
        {
            _repository = repository;
            _class_repository = class_repository;
        }

        public ActionResult Details(int id)
        {
            var student = _repository.GetStudentById(id);
            student.Class = _class_repository.GetClassById(student.ClassId);

            return View(student);
        }

        public ActionResult Create()
        {
            var classes = _class_repository.GetAll();
            ViewBag.Classes = classes;

            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentCreateViewModel model)
        {
            try
            {
                byte[] photoBytes = null;
                if (model.Image != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        model.Image.CopyTo(memory);
                        photoBytes = memory.ToArray();
                    }
                }

                var student = new Student()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    ClassId = model.ClassId,
                    HasDebt = model.HasDebt,
                    Level = model.Level,
                    Image = photoBytes
                };

                int id = _repository.Insert(student);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var student = _repository.GetStudentById(id);
            student.Class = _class_repository.GetClassById(student.ClassId);

            var classes = _class_repository.GetAll();
            ViewBag.Classes = classes;

            var model = new StudentCreateViewModel()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Email = student.Email,
                ClassId = student.ClassId,
                Phone = student.Phone,
                Level = student.Level,
                HasDebt = student.HasDebt,
                Class = student.Class
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentCreateViewModel model)
        {
            try
            {
                byte[] photoBytes = null;
                if (model.Image != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        model.Image.CopyTo(memory);
                        photoBytes = memory.ToArray();
                    }
                }

                var student = new Student()
                {
                    StudentId = id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Level = model.Level,
                    ClassId = model.ClassId,
                    Phone = model.Phone,
                    HasDebt = model.HasDebt,
                    Image = photoBytes
                };

                _repository.Update(student);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var student = _repository.GetStudentById(id);
            student.Class = _class_repository.GetClassById(student.ClassId);

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
            foreach (var student in students)
            {
                student.Class = _class_repository.GetClassById(student.ClassId);
            }

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
                    //_repository.ImportCSV(reader.ReadToEnd());
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

        public ActionResult ShowImage(int? id)
        {
            if (id.HasValue)
            {
                var teacher = _repository.GetStudentById(id ?? -1);
                if (teacher?.Image != null)
                {
                    return File(
                        teacher.Image,
                        "image/jpeg",
                        $"student_{id}.jpg");
                }
            }

            return NotFound();
        }
    }
}
