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
        // GET: ClassController
        public ActionResult Index()
        {
            var classes = _repository.GetAll();
            return View(classes);
        }

        // GET: ClassController/Details/5
        public ActionResult Details(int id)
        {
            var @class = _repository.GetClassById(id);
            return View(@class);
        }

        // GET: ClassController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ClassController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ClassController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ClassController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
