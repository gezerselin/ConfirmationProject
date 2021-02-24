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

namespace ConfirmationProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly ConfirmationProjectDbContext _context;

        public UsersController(ConfirmationProjectDbContext context)
        {
            _context = context;
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
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
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Redirect("/Account/Login");
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return Redirect("/Account/Login");
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
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
                    _context.Update(user);
                    await _context.SaveChangesAsync();
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
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }



        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult EditPassWord(string OldPassword, string newPassword, string newSamePassword)
        {
            int id = Convert.ToInt32(User.Identity.Name);
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (OldPassword == user.Password)
            {
                if (newPassword == newSamePassword)
                {
                    user.Password = newPassword;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
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
