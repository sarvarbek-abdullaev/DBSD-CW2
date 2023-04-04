using AdmissionSystem.DAL;
using AdmissionSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

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
        public ActionResult Create(TeacherCreateViewModel model)
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

                var teacher = new Teacher()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsMarried = model.IsMarried,
                    Salary = model.Salary,
                    Image = photoBytes
                };

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

            var model = new TeacherCreateViewModel()
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                BirthDate = teacher.BirthDate,
                Email = teacher.Email,
                Phone = teacher.Phone,
                IsMarried = teacher.IsMarried,
                Salary = teacher.Salary
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,TeacherCreateViewModel model)
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

                var teacher = new Teacher()
                {
                    TeacherId = id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsMarried = model.IsMarried,
                    Salary = model.Salary,
                    Image = photoBytes
                };
                _repository.Update(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
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

        public ActionResult ShowImage(int? id)
        {
            if (id.HasValue)
            {
                var teacher = _repository.GetTeacherById(id ?? -1);
                if (teacher?.Image != null)
                {
                    return File(
                        teacher.Image,
                        "image/jpeg",
                        $"teacher_{id}.jpg");
                }
            }

            return NotFound();
        }
    }
}
