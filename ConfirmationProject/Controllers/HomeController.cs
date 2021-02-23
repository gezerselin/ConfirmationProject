using ConfirmationProject.Data;
using ConfirmationProject.Models;
using ConfirmationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private ConfirmationProjectDbContext dbContext;

        public HomeController(ILogger<HomeController> logger , ISurveyService surveyService, ConfirmationProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.surveyService = surveyService;
            _logger = logger;
        }

        public IActionResult Index(int page=1)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            int totalSurvey = 0;
            var pageSize = 3;
            var surveys = surveyService.GetSurveys();
            List<Survey> pagingSurvey = new List<Survey>();

            foreach (var item in surveys)
            {
                if (((item.Responses.FirstOrDefault(x => x.UserId == UserId)) == null) && (DateTime.Now <= item.Deadline) && (item.numberOfYes < item.numberOfConfirmation))
                {
                    pagingSurvey.Add(item);
                    totalSurvey++;

                }
            }

            var totalPages = Math.Ceiling((decimal)totalSurvey / pageSize);
            ViewBag.totalPages=totalPages;

            return View(pagingSurvey);
        }

        public IActionResult AnsweredSurvey(int page=1)
        {

            int UserId = Convert.ToInt32(User.Identity.Name);
            int totalSurvey = 0;
            var pageSize = 3;
            var surveys = surveyService.GetSurveys();
            List<Survey> pagingSurvey = new List<Survey>();
            
            foreach (var item in surveys)
            {
                if ((item.Responses.FirstOrDefault(x => x.UserId == UserId)) != null)
                {
                    pagingSurvey.Add(item);
                    totalSurvey++;
                }
            }
            var pagingSurveys = pagingSurvey.OrderBy(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

            var totalPages = Math.Ceiling((decimal)totalSurvey / pageSize);
            ViewBag.totalPages = totalPages;

            return View(pagingSurveys);

        }

        public IActionResult TimeOutSurvey(int page=1)
        {

            int UserId = Convert.ToInt32(User.Identity.Name);
            int totalSurvey = 0;
            var pageSize = 3;
            var surveys = surveyService.GetSurveys();
            List<Survey> pagingSurvey = new List<Survey>();

            foreach (var item in surveys)
            {
                if (DateTime.Now > item.Deadline)
                {
                    pagingSurvey.Add(item);
                    totalSurvey++;

                }
            }
            var pagingSurveys = pagingSurvey.OrderBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            var totalPages = Math.Ceiling((decimal)totalSurvey / pageSize);
            ViewBag.totalPages = totalPages;

            return View(pagingSurveys);

        }

        public IActionResult ApprovedSurvey(int page=1)
        {
            
            int totalSurvey = 0;
            var pageSize = 3;
            var surveys = surveyService.GetSurveys();
            List<Survey> pagingSurvey = new List<Survey>();

            foreach (var item in surveys)
            {
                if (item.numberOfYes == item.numberOfConfirmation)
                {
                    pagingSurvey.Add(item);
                    totalSurvey++;
                }
            }
            var pagingSurveys = pagingSurvey.OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var totalPages = Math.Ceiling((decimal)totalSurvey / pageSize);
            ViewBag.totalPages = totalPages;

            return View(pagingSurveys);
        }
        public IActionResult Search(string searchItem, int page=1)

        {
            var surveys = from m in dbContext.Surveys.Include(x => x.Responses).ThenInclude(x => x.User) select m;
            if (!String.IsNullOrEmpty(searchItem))
            {
                surveys = surveys.Where(s => s.Title.Contains(searchItem));
            }
           

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
