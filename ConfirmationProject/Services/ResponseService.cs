using ConfirmationProject.Data;
using ConfirmationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public class ResponseService : IResponseService
    {
        private ConfirmationProjectDbContext dbContext;

        public ResponseService(ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void addResponse(Response response)
        {
            dbContext.Responses.Add(response);
            dbContext.SaveChanges();
        }

        public int EditSurvey(Survey survey)
        {
            dbContext.Entry(survey).State = EntityState.Modified;
            return dbContext.SaveChanges();
        }
    }
}
