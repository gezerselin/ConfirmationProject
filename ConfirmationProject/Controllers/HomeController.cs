using ConfirmationProject.Models;
using ConfirmationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ISurveyService surveyService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , ISurveyService surveyService)
        {
            this.surveyService = surveyService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var surveys = surveyService.GetSurveys();
            return View(surveys);
        }

        public IActionResult AnsweredSurvey()
        {
            var surveys = surveyService.GetSurveys();
            return View(surveys);
        }

        public IActionResult TimeOutSurvey()
        {
            var surveys = surveyService.GetSurveys();
            return View(surveys);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
