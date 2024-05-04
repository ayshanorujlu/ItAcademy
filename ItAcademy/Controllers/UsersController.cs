using ItAcademy.Helper;
using ItAcademy.Models;
using ItAcademy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItAcademy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<AppUser> _signInManager;
        public UsersController(UserManager<AppUser> userManager,
         RoleManager<IdentityRole> roleManager
         /*SignInManager<AppUser> signInManager*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> DbUsers =await _userManager.Users.OrderByDescending(x => x.Id).ToListAsync();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser Dbuser in DbUsers)
            {
                UserVM userVm = new UserVM
                {
                    Name = Dbuser.Name,
                    Email = Dbuser.Email,
                    Username = Dbuser.UserName,
                    IsDeactive = Dbuser.IsDeactive,
                    Id = Dbuser.Id,
                    Role = (await _userManager.GetRolesAsync(Dbuser))[0]
                };
                usersVM.Add(userVm);
            }
            return View(usersVM);
        }
        #region ResetPassword
        public async Task<IActionResult> ResertPassword(string id)
        {
            if (id == null)
                return BadRequest();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResertPassword(string id, ResetPasswordVM resetPasswordVM)
        {
            if (id == null)
                return NotFound();

            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Try again");

            //    return View();
            //}

            //AppUser user = await _userManager.FindByNameAsync(resetPasswordVM.Name);
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest();
            }
            //var result = await _userManager.ChangePasswordAsync(user, resetPasswordVM.OldPassword, resetPasswordVM.Password);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(Index), "Dashboard");
        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM createVM,string role)
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            AppUser NewUser = new AppUser
            {
                UserName = createVM.Username,
                Email = createVM.Email,
                Name = createVM.Name,


            };
            IdentityResult identityResult = await _userManager.CreateAsync(NewUser, createVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(NewUser, role);
            return RedirectToAction("Index");
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return BadRequest();
            }
            if (appUser.IsDeactive)
            {
                appUser.IsDeactive = false;
            }
            else
            {
                appUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(appUser);
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(string id)
        {
            #region GET
            if (id == null)
            {
                return NotFound();
            }
            AppUser DbUser = await _userManager.FindByIdAsync(id);
            if (DbUser == null)
            {
                return BadRequest();
            }
            UpdateVM DbupdateVM = new UpdateVM
            {
                Name = DbUser.Name,
                Username = DbUser.UserName,
                Email = DbUser.Email,
                Role = (await _userManager.GetRolesAsync(DbUser))[0]

            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            }; 
            #endregion
            return View(DbupdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM,string role)
        {
            #region GET
            if (id == null)
            {
                return NotFound();
            }
            AppUser DbUser = await _userManager.FindByIdAsync(id);
            if (DbUser == null)
            {
                return BadRequest();
            }
            UpdateVM DbupdateVM = new UpdateVM
            {
                Name = DbUser.Name,
                Username = DbUser.UserName,
                Email = DbUser.Email,
                Role = (await _userManager.GetRolesAsync(DbUser))[0]

            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            #endregion
            DbUser.Name = updateVM.Name;
            DbUser.UserName = updateVM.Username;
            DbUser.Email = updateVM.Email;
            if (DbupdateVM.Role != role)
            {
             IdentityResult removeIdentityResult  = await _userManager.RemoveFromRoleAsync(DbUser, DbupdateVM.Role);
                if (!removeIdentityResult.Succeeded) 
                {

                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(DbUser, role);
                if (!addIdentityResult.Succeeded)
                {

                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            await _userManager.UpdateAsync(DbUser);
           
            
            return RedirectToAction("Index");
        }
        #endregion
      

       
    }
}
