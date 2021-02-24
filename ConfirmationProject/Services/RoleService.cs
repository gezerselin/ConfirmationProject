using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public class RoleService : IRoleService
    {

        private ConfirmationProjectDbContext dbContext;
        public RoleService(ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Role GetRoleById(int id)
        {
            return dbContext.Roles.FirstOrDefault(x => x.Id == id); 
        }

        public IList<Role> GetRoles()
        {
            return dbContext.Roles.AsNoTracking().ToList();
        }
    }
}
