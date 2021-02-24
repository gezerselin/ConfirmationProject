using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public class GenderService : IGenderService
    {
        private ConfirmationProjectDbContext dbContext;
        public GenderService(ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IList<Gender> GetGenders()
        {
            return dbContext.Genders.AsNoTracking().ToList();
        }
    }
}
