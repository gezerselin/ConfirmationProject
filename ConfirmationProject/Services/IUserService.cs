using ConfirmationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public interface IUserService
    {
        User ValidUser(string UserName, string password);
    

        User GetUserById(int id);
        void AddUser(User user);
        void updateUser(User user);
        bool Exists(int id);

        IList<User> GetUsers();
    }
}
