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
        public ClassController(IClassRepository repository, ITeacherRepository teacher_repository)
        {
            _repository = repository;
            _teacher_repository = teacher_repository;
        }

        public ActionResult Index()
        {
            var classes = _repository.GetAll();

            foreach (var _class in classes)
            {
                _class.Teacher = _teacher_repository.GetTeacherById(_class.TeacherID);
            }

            return View(classes);
        }

        public ActionResult Details(int id)
        {
            var @class = _repository.GetClassById(id);
            @class.Teacher = _teacher_repository.GetTeacherById(@class.TeacherID);
            return View(@class);
        }

        public ActionResult Create()
        {
            var teachers = _teacher_repository.GetAll();
            ViewBag.Teachers = teachers;
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
