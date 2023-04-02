using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.IO;
using X.PagedList;

namespace AdmissionSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _repository;
        public TeacherController(ITeacherRepository repository)
        {
            _repository = repository;
        }

        // GET: TeacherController
        public ActionResult Index()
        {
            var teachers = _repository.GetAll();
            return View(teachers);
        }
        public ActionResult Filter(TeacherFilterViewModel filterModel)
        {
            int totalRows;
            var teachers = _repository.Filter(
                filterModel.FirstName, filterModel.LastName,
                out totalRows, filterModel.Page, filterModel.PageSize
                );

            filterModel.Teachers = teachers;

            return View(filterModel);
        }

        // GET: TeacherController/Details/5
        public ActionResult Details(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            return View(teacher);
        }

        // GET: TeacherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherCreateViewModel model)
        {
            try
            {
                byte[] photoBytes = null;
                if (model.Photo != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        model.Photo.CopyTo(memory);
                        photoBytes = memory.ToArray();
                    }
                }

                var teacher = new Teacher()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsMarried = model.IsMarried,
                    Salary = model.Salary,
                    Photo = photoBytes
                };

                int id = _repository.Insert(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Edit/5
        public ActionResult Edit(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            var model = new TeacherCreateViewModel()
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                BirthDate = teacher.BirthDate,
                Email = teacher.Email,
                Phone = teacher.Phone,
                IsMarried = teacher.IsMarried,
                Salary = teacher.Salary,
            };

            return View(model);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeacherCreateViewModel model)
        {
            try
            {
                byte[] photoBytes = null;
                if (model.Photo != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        model.Photo.CopyTo(memory);
                        photoBytes = memory.ToArray();
                    }
                }
                
                var teacher = new Teacher()
                {
                    TeacherId = model.TeacherId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsMarried = model.IsMarried,
                    Salary = model.Salary,
                    Photo = photoBytes
                };

                _repository.Update(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Delete/5
        public ActionResult Delete(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            return View(teacher);
        }

        // POST: TeacherController/Delete/5
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

        public ActionResult ShowImage(int? id)
        {
            if (id.HasValue)
            {
                var emp = _repository.GetTeacherById(id ?? -1);
                if (emp?.Photo != null)
                {
                    return File(
                        emp.Photo,
                        "image/jpeg",
                        $"employee_{id}.jpg");
                }
            }

            return NotFound();
        }
    }
}
