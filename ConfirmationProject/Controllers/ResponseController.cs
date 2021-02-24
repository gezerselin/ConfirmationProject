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
        private IResponseService responseService;
        private ISurveyService surveyService;
        private IUserService userService;
      
        public ResponseController( IResponseService responceService,ISurveyService surveyService, IUserService userService)
        {
            this.responseService = responceService;
            this.userService = userService;
            this.surveyService = surveyService;
            
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
                var survey = surveyService.GetSurveysById(response.SurveyId);
       
                survey.numberOfYes += 1;

                responseService.EditSurvey(survey);
                
            }else if (inlineRadioOptions == "no")
            {
                var survey = surveyService.GetSurveysById(response.SurveyId);
                survey.numberOfNo += 1;

                responseService.EditSurvey(survey);
            }

            var SelectedSurvey = surveyService.GetSurveysById(surveyid);

            var user = userService.GetUserById(SelectedSurvey.CreatorId);

            var UserInfo = userService.GetUserById((Convert.ToInt32(User.Identity.Name)));

          

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
            objeto_mail.Subject = $"Yeni Yanıt - ({SelectedSurvey.Title})";
            objeto_mail.Body = $"Başlık : {SelectedSurvey.Title} \n" +
                $"Detay:{SelectedSurvey.Detail} \n\n" +
                $"Yanıt veren : {UserInfo.Name} {UserInfo.LastName} \n" +
                $"İletişim:\n {UserInfo.Email}\n " +
                $"{UserInfo.PhoneNumber}\n\n" +
                $"Cevabı : {response.Answer} \n" +
                $"Notu: {response.Note}\n\n" +
                $"Kabul sayısı : {SelectedSurvey.numberOfYes}  Red sayısı : {SelectedSurvey.numberOfNo} \n" +
                $"Kabul eden sayısı/Onay sayısı: %{requiredConfirmation} \nKabul için kalan onay sayısı: {(SelectedSurvey.numberOfConfirmation-SelectedSurvey.numberOfYes)} ";
            client.Send(objeto_mail);

            return Redirect("/");
        }
        
        public IActionResult CreateResponse(int surveyid)
        {

            var SelectedSurvey = surveyService.GetSurveysById(surveyid);


            return View(SelectedSurvey);
        }
    }
}
