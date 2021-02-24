using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.AspNetCore.Authorization;
using ConfirmationProject.Services;
using System.Security;

namespace ConfirmationProject.Controllers
{
    public class UsersController : Controller
    {
        private IRoleService roleService;
        private IGenderService genderService;
        private IUserService userService;
        

        public UsersController( IRoleService roleService,IGenderService genderService, IUserService userService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.genderService = genderService;
           
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(genderService.GetGenders(), "Id", "Name");
            ViewData["RoleId"] = new SelectList(roleService.GetRoles(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Email,UserName,Password,PhoneNumber,RoleId,GenderId,MailConfirmation")] User user)
        {
            if (ModelState.IsValid)
            {
                userService.AddUser(user);
                
              
                return Redirect("/Account/Login");
            }
            ViewData["GenderId"] = new SelectList(genderService.GetGenders(), "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(roleService.GetRoles(), "Id", "Name", user.RoleId);
            return Redirect("/Account/Login");
        }


        [Authorize]
        // GET: Users/Edit/5
        public IActionResult Edit(int id)
        {

            //Make sure the submission belongs to the user
            if ((userService.GetUserById(id)).Id != (Convert.ToInt32(User.Identity.Name)))
            {
                return NotFound();
            }

            var user = userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(genderService.GetGenders(), "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(roleService.GetRoles(), "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Email,UserName,Password,PhoneNumber,RoleId,GenderId,MailConfirmation")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userService.updateUser(user);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string url = $"/Users/Edit/{(Convert.ToInt32(User.Identity.Name))}";
                return Redirect(url);
            }
            ViewData["GenderId"] = new SelectList(genderService.GetGenders(), "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(roleService.GetRoles(), "Id", "Name", user.RoleId);
            return View(user);
        }



        private bool UserExists(int id)
        {
            return userService.Exists(id);
            
        }

        [HttpPost]
        public IActionResult EditPassWord(string OldPassword, string newPassword, string newSamePassword)
        {
            int id = Convert.ToInt32(User.Identity.Name);
            var user = userService.GetUserById(id);

            if (OldPassword == user.Password)
            {
                if (newPassword == newSamePassword)
                {
                    user.Password = newPassword;
                    userService.updateUser(user);
                    return Json("Şifreniz Güncellendi");
                }
                else
                {
                    return Json("Yeni şifreler aynı olmalıdır");
                }

            }
            else
            {
                return Json("Eski şifreniz yanlış");
            }



        }
    }
}
