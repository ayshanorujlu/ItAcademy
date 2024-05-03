using ItAcademy.DAL;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    public class CoursesController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly IWebHostEnvironment _env;
        public CoursesController(AppDbContext Db, IWebHostEnvironment env)
        {

            _Db = Db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
          
            List<Courses> courses = await _Db.Courses.OrderByDescending(x => x.Id).ToListAsync();

            return View(courses);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Courses courses)
        {
            bool isExist = await _Db.Courses.AnyAsync(x => x.Name == courses.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu kurs artıq mövcuddur !");
                return View(courses);
            }
            await _Db.Courses.AddAsync(courses);
            await _Db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Courses _DbCourses = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbCourses == null)
            {
                return BadRequest();
            }
            return View(_DbCourses);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Courses courses)
        {
            if (id == null)
            {
                return NotFound();
            }
            Courses _DbCourses = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbCourses == null)
            {
                return BadRequest();
            }
            bool isExist = await _Db.Courses.AnyAsync(x => x.Name == courses.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This courses is already exist !");
                return View(courses);
            }

            _DbCourses.Name = courses.Name;
            _DbCourses.Mark = courses.Mark;
          
            _DbCourses.CoursesDuration = courses.CoursesDuration;
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Detail(int? id)
        {

            Courses courses = await _Db.Courses.FindAsync(id);


            //_DbCourses.Name = service.Name;
            //_DbCourses.Description = service.Description;

            return View(courses);
        }
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Courses _DbCourses = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
        //    if (_DbCourses == null)
        //    {
        //        return BadRequest();
        //    }
        //    return View(_DbCourses);

        //}
        //[HttpPost]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeletePost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Courses _DbCourses = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
        //    if (_DbCourses == null)
        //    {
        //        return BadRequest();
        //    }
        //    _DbCourses.IsDeactive = true;
        //    await _Db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Courses _DbCourses = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbCourses == null)
            {
                return BadRequest();
            }
            if (_DbCourses.IsDeactive)
            {
                _DbCourses.IsDeactive = false;
            }
            else
            {
                _DbCourses.IsDeactive = true;
            }
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
