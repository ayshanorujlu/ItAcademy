using ItAcademy.DAL;
using ItAcademy.Helper;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    public class DashBoardController : Controller
    {
        private readonly AppDbContext _Db;
        public DashBoardController(AppDbContext Db)
        {
                _Db = Db;
        }

        public async Task<IActionResult> Index()
        {
            List<Courses> courses = await  _Db.Courses.OrderByDescending(x => x.Id).ToListAsync();

            return View(courses);
        }

       
        public IActionResult Error()
        {
            return View();
        }
    }
}
