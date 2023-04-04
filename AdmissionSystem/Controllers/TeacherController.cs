using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _repository;
        public TeacherController(ITeacherRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var teachers = _repository.GetAll();
            return View(teachers);
        }

        public ActionResult Filter(TeacherFilterViewModel filterModel)
        {
            int totalRows;
            var teachers = _repository.Filter(
                filterModel.FirstName, filterModel.LastName, filterModel.Page, filterModel.PageSize
                );

            filterModel.Teachers = teachers;

            return View(filterModel);
        }

        public ActionResult Details(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            return View(teacher);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Teacher teacher)
        {
            try
            {
                int id = _repository.Insert(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            return View(teacher);
        }

        [HttpPost]
        public ActionResult Edit(int id, Teacher teacher)
        {
            try
            {
                teacher.TeacherId = id;
                _repository.Update(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            return View(teacher);
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
    }
}
