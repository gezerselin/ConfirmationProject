using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public class UserService : IUserService
    {
        private ConfirmationProjectDbContext dbContext;

        public UserService(ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public User ValidUser(string UserName, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.UserName == UserName && u.Password == password);
        }


        public User GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public void AddUser(User user)
        {
            dbContext.Users.Add(user);

            dbContext.SaveChanges();
        }

        public void updateUser(User user)
        {

            dbContext.Entry(user).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public bool Exists(int id)
        {
            return dbContext.Users.Any(e => e.Id == id);
        }

        public IList<User> GetUsers()
        {
            return dbContext.Users.AsNoTracking().ToList();
        }
    }
}
