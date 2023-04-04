using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassRepository _repository;
        private readonly ITeacherRepository _teacher_repository;
        private readonly ICourseRepository _course_repository;
        public ClassController(IClassRepository repository, ITeacherRepository teacher_repository, ICourseRepository course_repository)
        {
            _repository = repository;
            _teacher_repository = teacher_repository;
            _course_repository = course_repository;
        }

        public ActionResult Index()
        {
            var classes = _repository.GetAll();

            foreach (var _class in classes)
            {
                _class.Teacher = _teacher_repository.GetTeacherById(_class.TeacherID);
                _class.Course = _course_repository.GetCourseById(_class.CourseId);
            }

            return View(classes);
        }

        public ActionResult Details(int id)
        {
            var @class = _repository.GetClassById(id);
            @class.Teacher = _teacher_repository.GetTeacherById(@class.TeacherID);
            @class.Course = _course_repository.GetCourseById(@class.CourseId);
            return View(@class);
        }

        public ActionResult Create()
        {
            var teachers = _teacher_repository.GetAll();
            var courses = _course_repository.GetAll();
            ViewBag.Teachers = teachers;
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Class @class)
        {
            try
            {
                int id = _repository.Insert(@class);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
