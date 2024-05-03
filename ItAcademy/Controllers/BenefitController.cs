using ItAcademy.DAL;
using ItAcademy.Helper;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    public class BenefitController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly UserManager<AppUser> _userManager;
        public BenefitController(AppDbContext Db, UserManager<AppUser> userManager)
        {
                _Db = Db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Benefit> benefits= await _Db.Benefits.OrderByDescending(x => x.Id).ToListAsync();
            
           


            return View(benefits);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Benefit benefit)
        {
            if (benefit.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Məbləğ düzgün daxil edilməyib.");
                return View();
            }
            benefit.By = User.Identity.Name;
            Budget budget = await _Db.Budgets.FirstOrDefaultAsync();
            budget.LastModifiedDescription = benefit.Description;
            budget.LastModifiedDate = DateTime.UtcNow.AddHours(4);
            budget.LastModifiedAmount = benefit.Amount;
            budget.LastModifiedBy = benefit.By;
            budget.TotalBudget += benefit.Amount;

            await _Db.Benefits.AddAsync(benefit);
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
