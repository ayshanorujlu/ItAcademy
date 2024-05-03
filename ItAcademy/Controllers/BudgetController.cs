using ItAcademy.DAL;
using ItAcademy.Helper;
using ItAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Member,Admin")]
    public class BudgetController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly UserManager<AppUser> _userManager;
        public BudgetController(AppDbContext Db, UserManager<AppUser> userManager)
        {
                _Db = Db;
            _userManager= userManager;
    }
        public async Task<IActionResult> Index()
        {
            Budget budget = await _Db.Budgets.FirstOrDefaultAsync();
            return View(budget);
        }
    }
}
