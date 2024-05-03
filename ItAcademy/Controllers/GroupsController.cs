using ItAcademy.DAL;
using ItAcademy.Helper;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]

    public class GroupsController : Controller
    {

        private readonly AppDbContext _Db;
        private readonly IWebHostEnvironment _env;

       

        public GroupsController(AppDbContext Db, IWebHostEnvironment env)
        {

            _Db = Db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            //List<Groups> groups = await _Db.Groups.Include(x => x.Courses).OrderByDescending(x => x.Id).ToListAsync();

            //return View(groups);
            var groups = await _Db.Groups
           .Include(s => s.GroupStudent)
               .ThenInclude(sg => sg.Students)
           .Include(s => s.Courses).OrderByDescending(x => x.Id)
           .ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();

            return View(groups);
        }
        #region create
        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _Db.Courses.ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Groups groups, int CatId)
        {
            ViewBag.Courses = await _Db.Courses.ToListAsync();
           

            //    #region Save Image


            //    if (groups.Photo == null)
            //    {
            //        ModelState.AddModelError("Photo", "Image can't be null!!");
            //        return View();
            //    }
            //    if (!groups.Photo.IsImage())
            //    {
            //        ModelState.AddModelError("Photo", "Please select image type");
            //        return View();
            //    }
            //    if (groups.Photo == null)
            //    {
            //        ModelState.AddModelError("Photo", "max 1mb !!");
            //        return View();
            //    }

            //    string folder = Path.Combine(_env.WebRootPath, "images");
            //groups.Image = await groups.Photo.SaveFileAsync(folder);
            //    #endregion
            #region Exist Item
            bool isExist = await _Db.Groups.AnyAsync(x => x.Name == groups.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This groups is already exist !");
                return View(groups);
            }
            #endregion
            groups.CoursesId = CatId;
           
            await _Db.Groups.AddAsync(groups);
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Groups _DbGroups = await _Db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbGroups == null)
            {
                return BadRequest();
            }
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            return View(_DbGroups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Groups groups, int CatId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Groups _DbGroups = await _Db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbGroups == null)
            {
                return BadRequest();
            }
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            //#region Exist Item
            //bool isExist = await _Db.Teachers.AnyAsync(x => x.Name == teachers.Name && CatId != id);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "This teachers is already exist !");
            //    return View(teachers);
            //}
            //#endregion
            //#region Save Image


            //if (groups.Photo != null)
            //{
            //    if (!groups.Photo.IsImage())
            //    {
            //        ModelState.AddModelError("Photo", "Şəkil seçin!!");
            //        return View();
            //    }
            //    if (teachers.Photo == null)
            //    {
            //        ModelState.AddModelError("Photo", "max 1mb !!");
            //        return View();
            //    }
            //    string folder = Path.Combine(_env.WebRootPath, "images");
            //    _DbGroups.Image = await teachers.Photo.SaveFileAsync(folder);

            //}

            //#endregion
            _DbGroups.Name = groups.Name;



            _DbGroups.CoursesId = CatId;

            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Groups _DbGroups = await _Db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbGroups == null)
            {
                return BadRequest();
            }
            if (_DbGroups.IsDeactive)
            {
                _DbGroups.IsDeactive = false;
            }
            else
            {
                _DbGroups.IsDeactive = true;
            }
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        public async Task<IActionResult> Student(int id)
        {

            var groups = await _Db.Groups
            .Include(s => s.GroupStudent)
                .ThenInclude(sg => sg.Students).FirstOrDefaultAsync();


            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();


            return View(groups);
        }

    }
}
