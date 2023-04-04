using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _repository;
        public CourseController(ICourseRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var courses = _repository.GetAll();
            return View(courses);
        }

        public ActionResult Details(int id)
        {
            var course = _repository.GetCourseById(id);
            return View(course);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Course course)
        {
            try
            {
                int id = _repository.Insert(course);
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
