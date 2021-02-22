using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public class SurveyService : ISurveyService
    {
        private ConfirmationProjectDbContext dbContext;
        public SurveyService(ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Survey> GetSurveys()
        {
            var surveys = dbContext.Surveys.Include(x=>x.Responses).ThenInclude(x=>x.User).AsNoTracking().ToList();
            return surveys;
        }

    }
}
