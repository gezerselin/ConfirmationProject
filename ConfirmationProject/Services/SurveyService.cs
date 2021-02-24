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

        public void AddSurvey(Survey survey)
        {
            dbContext.Surveys.Add(survey);
            dbContext.SaveChanges();
        }

        public void deleteSurvey(Survey survey)
        {
            dbContext.Surveys.Remove(survey);
            dbContext.SaveChanges();
        }

        public bool Exists(int id)
        {
            return dbContext.Surveys.Any(e => e.Id == id);
        }

        public List<Survey> GetSurveys()
        {
            var surveys = dbContext.Surveys.Include(x=>x.Responses).ThenInclude(x=>x.User).AsNoTracking().ToList();
            return surveys;
        }

        public Survey GetSurveysById(int id)
        {
            return dbContext.Surveys.FirstOrDefault(x => x.Id==id);
        }

        public void UpdateSurvey(Survey survey)
        {
            dbContext.Entry(survey).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}
