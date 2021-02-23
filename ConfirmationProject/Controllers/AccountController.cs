using ConfirmationProject.Data;
using ConfirmationProject.Models;
using ConfirmationProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConfirmationProject.Controllers
{
    
    public class AccountController : Controller
    {
        private ConfirmationProjectDbContext dbContext;
        private IUserService userService;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public AccountController(ConfirmationProjectDbContext dbContext, IUserService userService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            var user = userService.ValidUser(userLoginModel.UserName, userLoginModel.Password);
            if (user != null)
            {
                List<Claim> claims = new List<Claim>();


                var role = dbContext.Roles.FirstOrDefault(x => x.Id == user.RoleId);

                claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, role.Name));



                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

   
                return Redirect("/");


            }
            else
            {
                ModelState.AddModelError("hata", "Kullanıcı adı veya şifre hatalı");
                return View();
            }


        }


        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");


        }
    }
}
