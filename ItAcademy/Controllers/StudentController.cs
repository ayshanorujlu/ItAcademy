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

    public class StudentController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly IWebHostEnvironment _env;
        public StudentController(AppDbContext Db, IWebHostEnvironment env)
        {

            _Db = Db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            //  List<Students> students = await _Db.Students.Include(x => x.Courses).Include(x=> x.GroupStudent).ThenInclude(x=> x.Groups).ToListAsync();

            //   List<GroupStudent> groupStudents = await _Db.GroupStudent.Include(x => x.Groups).Include(x=> x.Students.).ToListAsync();

            var students = await _Db.Students
            .Include(s => s.GroupStudent)
                .ThenInclude(sg => sg.Groups)
            .Include(s => s.Courses).OrderByDescending(x => x.Id)
            .ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
          
            return View(students);
        }
        #region create
        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.Groups = await _Db.Groups.ToListAsync();

            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
            //ViewBag.StudentGroups = await _Db.StudentGroups.ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Students students, int CatId,int GatId)
        {
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.Groups = await _Db.Groups.ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
            //ViewBag.StudentGroups = await _Db.StudentGroups.ToListAsync();

            #region Save Image


            if (students.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can't be null!!");
                return View();
            }
            if (!students.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (students.Photo == null)
            {
                ModelState.AddModelError("Photo", "max 1mb !!");
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "images");
            students.Image = await students.Photo.SaveFileAsync(folder);
            #endregion

            #region Exist Item
            bool isExist = await _Db.Students.AnyAsync(x => x.Name == students.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This students is already exist !");
                return View(students);
            }
            #endregion

            var GroupStudent = new List<GroupStudent>();
            var groupStudent = new GroupStudent
            {
                StudentsId = students.Id,
                GroupsId = GatId
            };
            GroupStudent.Add(groupStudent);
            students.GroupStudent = GroupStudent;

            students.CoursesId = CatId;

            await _Db.Students.AddAsync(students);
           // await _Db.GroupStudent.AddAsync(groupStudent);
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
            Students _DbStudents = await _Db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbStudents == null)
            {
                return BadRequest();
            }
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
            ViewBag.Groups = await _Db.Groups.ToListAsync();
            return View(_DbStudents);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Students students, int CatId , int GatId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Students _DbStudents = await _Db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbStudents == null)
            {
                return BadRequest();
            }

            GroupStudent groupStudent = await _Db.GroupStudent.Where(x=> x.StudentsId == id).FirstOrDefaultAsync();
            if (groupStudent == null)
            {
                return BadRequest();
            }

            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
            ViewBag.Groups = await _Db.Groups.ToListAsync();


            //#region Exist Item
            //bool isExist = await _Db.Teachers.AnyAsync(x => x.Name == teachers.Name && CatId != id);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "This teachers is already exist !");
            //    return View(teachers);
            //}
            //#endregion
            #region Save Image


            if (students.Photo != null)
            {
                if (!students.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Şəkil seçin!!");
                    return View();
                }
                if (students.Photo == null)
                {
                    ModelState.AddModelError("Photo", "max 1mb !!");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "images");
                _DbStudents.Image = await students.Photo.SaveFileAsync(folder);

            }

            #endregion
            //var GroupStudent = new List<GroupStudent>();
            //var groupStudent = new GroupStudent
            //{
            //    StudentsId = students.Id,
            //    GroupsId = GatId
            //};
            //GroupStudent.Add(groupStudent);
            _DbStudents.Name = students.Name;
            _DbStudents.Email = students.Email;
            _DbStudents.Payment = students.Payment;
            _DbStudents.Birthday = students.Birthday;
            _DbStudents.Mobil = students.Mobil;
            groupStudent.GroupsId = GatId;

            //_DbStudents.GroupStudent. = GatId;
            _DbStudents.CoursesId = CatId;
            
            //students.GroupStudentId = GatId;

            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        public async Task<IActionResult> Detail(int id)
        {
            var students = await _Db.Students
            .Include(s => s.GroupStudent)
                .ThenInclude(sg => sg.Groups).Where(x=>x.Id==id).FirstOrDefaultAsync();
           
           
            ViewBag.Courses = await _Db.Courses.ToListAsync();
            ViewBag.GroupStudent = await _Db.GroupStudent.ToListAsync();
           

            return View(students);
        }
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Students _DbStudents = await _Db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbStudents == null)
            {
                return BadRequest();
            }
            if (_DbStudents.IsDeactive)
            {
                _DbStudents.IsDeactive = false;
            }
            else
            {
                _DbStudents.IsDeactive = true;
            }
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        public async Task<IActionResult> SendStudentSms(int id)
        {
            var student = _Db.Students.Where(x=> x.Id == id).FirstOrDefaultAsync();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendStudentSms(int id, string subject, string text)
        {
            var student = _Db.Students.Where(x => x.Id == id).FirstOrDefaultAsync();
            var email = student.Result.Email;
            await Sms.SendMailAsync(subject, text, email);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendAllStudentSms()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAllStudentSms(string subject, string text)
        {
            List<Students> students = await _Db.Students.ToListAsync();

            foreach (var student in students)
            {
                await Sms.SendMailAsync(subject, text, student.Email);
            }

            return RedirectToAction("Index");
        }

    }
}
