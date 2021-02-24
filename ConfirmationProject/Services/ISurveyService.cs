using ConfirmationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Services
{
    public interface ISurveyService
    {
        List<Survey> GetSurveys();
        Survey GetSurveysById(int id);

        void AddSurvey(Survey survey);
        void UpdateSurvey(Survey survey);
        void deleteSurvey(Survey survey);
        bool Exists(int id);
    }
}
