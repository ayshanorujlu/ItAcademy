using ItAcademy.Helper;
using ItAcademy.Models;
using ItAcademy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{


    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
         RoleManager<IdentityRole> roleManager,
         SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);
            if (User == null)
            {
                ModelState.AddModelError("", "İstifadəci adı və ya şifrə yanlışdır");
                return View();
            }
            if (user.IsDeactive)
            {
                ModelState.AddModelError("", "İstifadəçi adı tapılmadı");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemember, true);
            if (signInResult.IsLockedOut)
            {


                ModelState.AddModelError("", "İstifadəçi adı bloklanmışdır");
                return View();

            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "İstifadəci adı və ya şifrə yalnışdır");
                return View();

            }
            return RedirectToAction("Index", "DashBoard");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            AppUser NewUser = new AppUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
                Name = registerVM.Name,


            };
            IdentityResult identityResult = await _userManager.CreateAsync(NewUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(NewUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(NewUser, registerVM.IsRemember);
            return RedirectToAction("Index", "DashBoard");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //public async Task  CreateRoles()
        //{
        //    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
        //    {
        //        await  _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
        //    }
        //}

    }
}
