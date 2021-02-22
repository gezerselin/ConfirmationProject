using ConfirmationProject.Data;
using ConfirmationProject.Models;
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
    }
}
