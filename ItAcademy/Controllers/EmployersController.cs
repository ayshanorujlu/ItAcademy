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

    public class EmployersController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly IWebHostEnvironment _env;
        public EmployersController(AppDbContext Db, IWebHostEnvironment env)
        {

            _Db = Db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Employers> employers = await _Db.Employers.Include(x => x.Positions).OrderByDescending(x => x.Id).ToListAsync();
            return View(employers);
        }
       
        #region create
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _Db.Positions.ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employers employers, int CatId)
        {
            ViewBag.Positions = await _Db.Positions.ToListAsync();

            #region Save Image


            if (employers.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can't be null!!");
                return View();
            }
            if (!employers.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (employers.Photo == null)
            {
                ModelState.AddModelError("Photo", "max 1mb !!");
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "images");
            employers.Image = await employers.Photo.SaveFileAsync(folder);
            #endregion
            #region Exist Item
            bool isExist = await _Db.Teachers.AnyAsync(x => x.Name == employers.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This teachers is already exist !");
                return View(employers);
            }
            #endregion
            employers.PositionsId = CatId;
            await _Db.Employers.AddAsync(employers);
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
            Employers _DbEmployers = await _Db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbEmployers == null)
            {
                return BadRequest();
            }
            ViewBag.Positions = await _Db.Positions.ToListAsync();
            return View(_DbEmployers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Employers employers, int CatId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employers _DbEmployers = await _Db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbEmployers == null)
            {
                return BadRequest();
            }
            ViewBag.Positions = await _Db.Positions.ToListAsync();
            //#region Exist Item
            //bool isExist = await _Db.Teachers.AnyAsync(x => x.Name == teachers.Name && CatId != id);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "This teachers is already exist !");
            //    return View(teachers);
            //}
            //#endregion
            #region Save Image


            if (employers.Photo != null)
            {
                if (!employers.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image type");
                    return View();
                }
                if (employers.Photo == null)
                {
                    ModelState.AddModelError("Photo", "max 1mb !!");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "images");
                _DbEmployers.Image = await employers.Photo.SaveFileAsync(folder);

            }

            #endregion
            _DbEmployers.Name = employers.Name;
            _DbEmployers.Email = employers.Email;
            _DbEmployers.Salary = employers.Salary;
            _DbEmployers.Birthday = employers.Birthday;
            _DbEmployers.PhoneNumber = employers.PhoneNumber;

            _DbEmployers.PositionsId = CatId;

            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        public async Task<IActionResult> Detail(int id)
        {
           Employers employers = await _Db.Employers.FindAsync(id);
            ViewBag.Positions = await _Db.Positions.ToListAsync();
            return View(employers);
        }
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employers _DbEmployers = await _Db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbEmployers == null)
            {
                return BadRequest();
            }
            if (_DbEmployers.IsDeactive)
            {
                _DbEmployers.IsDeactive = false;
            }
            else
            {
                _DbEmployers.IsDeactive = true;
            }
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        public async Task<IActionResult> SendEmployersSms(int id)
        {
            var employers = _Db.Employers.Where(x => x.Id == id).FirstOrDefaultAsync();

            return View(employers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmployersSms(int id, string subject, string text)
        {
            var employers = _Db.Employers.Where(x => x.Id == id).FirstOrDefaultAsync();
            var email = employers.Result.Email;
            await Sms.SendMailAsync(subject, text, email);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendAllEmployersSms()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAllEmployersSms(string subject, string text)
        {
            List<Employers> employers = await _Db.Employers.ToListAsync();

            foreach (var employer in employers)
            {
                await Sms.SendMailAsync(subject, text, employer.Email);
            }

            return RedirectToAction("Index");
        }
    }
}
