using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _repository;
        private readonly IClassRepository _class_repository;
        public StudentController(IStudentRepository repository, IClassRepository class_repository)
        {
            _repository = repository;
            _class_repository = class_repository;
        }
        // GET: StudentController
        public ActionResult Index()
        {
            var students = _repository.GetAll();

            foreach (var student in students)
            {
                student.Class = _class_repository.GetClassById(student.ClassId);
            }

            return View(students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            var student = _repository.GetStudentById(id);
            student.Class = _class_repository.GetClassById(student.ClassId);

            return View(student);
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            var classes = _class_repository.GetAll();
            ViewBag.Classes = classes;

            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            var student = _repository.GetStudentById(id);

            var classes = _class_repository.GetAll();
            ViewBag.Classes = classes;

            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            var student = _repository.GetStudentById(id);
            student.Class = _class_repository.GetClassById(student.ClassId);
            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
