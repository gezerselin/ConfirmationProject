using ConfirmationProject.Data;
using ConfirmationProject.Models;
using ConfirmationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace ConfirmationProject.Controllers
{
    [Authorize]
    public class ResponseController : Controller
    {
        private ConfirmationProjectDbContext dbContext;
        private IResponseService responseService;
      
        public ResponseController(ConfirmationProjectDbContext dbContext, IResponseService responceService)
        {
            this.responseService = responceService;
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddResponse( string inlineRadioOptions, string note, int surveyid, int userid)
        {
            Response response = new Response();
            response.UserId = userid;
            response.SurveyId = surveyid;
            response.Note = note;
            response.Answer = inlineRadioOptions;
            responseService.addResponse(response);

            if (inlineRadioOptions == "yes")
            {
                var survey = dbContext.Surveys.FirstOrDefault(x => x.Id == response.SurveyId);
                survey.numberOfYes += 1;

                responseService.EditSurvey(survey);
                
            }else if (inlineRadioOptions == "no")
            {
                var survey = dbContext.Surveys.FirstOrDefault(x => x.Id == response.SurveyId);
                survey.numberOfNo += 1;

                responseService.EditSurvey(survey);
            }

            var SelectedSurvey = dbContext.Surveys.FirstOrDefault(x => x.Id == surveyid);
            var user = dbContext.Users.FirstOrDefault(x => x.Id == SelectedSurvey.CreatorId);

            var UserInfo = dbContext.Users.FirstOrDefault(x => x.Id == (Convert.ToInt32(User.Identity.Name)));

            double requiredConfirmation = Math.Round((((double)SelectedSurvey.numberOfYes / (double)SelectedSurvey.numberOfConfirmation) * 100), 2);

            MailMessage objeto_mail = new MailMessage();

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("selingezr@gmail.com", "gefzzofrfjxnwuql");
            objeto_mail.From = new MailAddress("selingezr@gmail.com");
            objeto_mail.To.Add(new MailAddress(user.Email));
            objeto_mail.Subject = "Yeni Onay";
            objeto_mail.Body = $" Onay veren : {UserInfo.Name} {UserInfo.LastName} \n" +
                $"İletişim:\n {UserInfo.Email}\n " +
                $"{UserInfo.PhoneNumber}\n" +
                $"Cevabı : {response.Answer} \n" +
                $"Notu: {response.Note}\n" +
                $"Kabul sayısı : {SelectedSurvey.numberOfYes}  Red sayısı : {SelectedSurvey.numberOfNo} \n" +
                $"Kabul eden sayısı/Onay sayısı: %{requiredConfirmation} \nKabul için kalan onay sayısı: {(SelectedSurvey.numberOfConfirmation-SelectedSurvey.numberOfYes)} ";
            client.Send(objeto_mail);

            return Redirect("/");
        }
        
        public IActionResult CreateResponse(int surveyid)
        {

            var SelectedSurvey = dbContext.Surveys.FirstOrDefault(x => x.Id == surveyid);

           
            return View(SelectedSurvey);
        }
    }
}
