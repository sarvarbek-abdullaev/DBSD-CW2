using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassRepository _repository;
        public ClassController(IClassRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var classes = _repository.GetAll();
            return View(classes);
        }

        public ActionResult Details(int id)
        {
            var @class = _repository.GetClassById(id);
            return View(@class);
        }

        public ActionResult Create()
        {
            
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
