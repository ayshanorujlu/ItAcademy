using ItAcademy.DAL;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    
    public class PositionsController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly IWebHostEnvironment _env;
        public PositionsController(AppDbContext Db, IWebHostEnvironment env)
        {

            _Db = Db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Positions> positions = await _Db.Positions.Include(x=>x.Employers).OrderByDescending(x => x.Id).ToListAsync();

            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Positions positions)
        {
            bool isExist = await _Db.Positions.AnyAsync(x => x.Name == positions.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu vəzifə artıq mövcüddür !");
                return View(positions);
            }
            await _Db.Positions.AddAsync(positions);
            await _Db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Positions _DbPositions = await _Db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbPositions == null)
            {
                return BadRequest();
            }
            return View(_DbPositions);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Positions positions)
        {
            if (id == null)
            {
                return NotFound();
            }
            Positions _DbPositions = await _Db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbPositions == null)
            {
                return BadRequest();
            }
            bool isExist = await _Db.Positions.AnyAsync(x => x.Name == positions.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Vəzifə artıq mövcüddür !");
                return View(positions);
            }

            _DbPositions.Name = positions.Name;
          
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {

            Positions positions = await _Db.Positions.FindAsync(id);


            //_DbPositions.Name = service.Name;
            //_DbPositions.Description = service.Description;

            return View(positions);
        }
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Courses _DbPositions = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
        //    if (_DbPositions == null)
        //    {
        //        return BadRequest();
        //    }
        //    return View(_DbPositions);

        //}
        //[HttpPost]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeletePost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Courses _DbPositions = await _Db.Courses.FirstOrDefaultAsync(x => x.Id == id);
        //    if (_DbPositions == null)
        //    {
        //        return BadRequest();
        //    }
        //    _DbPositions.IsDeactive = true;
        //    await _Db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Positions _DbPositions = await _Db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbPositions == null)
            {
                return BadRequest();
            }
            if (_DbPositions.IsDeactive)
            {
                _DbPositions.IsDeactive = false;
            }
            else
            {
                _DbPositions.IsDeactive = true;
            }
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
