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
    }
}
