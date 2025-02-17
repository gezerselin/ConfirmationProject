﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConfirmationProject.Data;
using ConfirmationProject.Models;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using ConfirmationProject.Services;

namespace ConfirmationProject.Controllers
{

    [Authorize(Roles = "Admin")]
    public class SurveysController : Controller
    {
        private ISurveyService surveyService;
        private IUserService userService;
        public SurveysController( ISurveyService surveyService, IUserService userService)
        {
            this.userService = userService;
            this.surveyService=surveyService;
           
        }


        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }



        // GET: Surveys
        public IActionResult Index(string sortOrder, string SearchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var surveys = from m in surveyService.GetSurveys()
                         select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                surveys = surveys.Where(s => s.Title.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    surveys = surveys.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    surveys = surveys.OrderBy(s => s.Deadline);
                    break;
                case "date_desc":
                    surveys = surveys.OrderByDescending(s => s.Deadline);
                    break;
                default:
                    surveys = surveys.OrderBy(s => s.Title);
                    break;
            }

            return View(surveys.ToList());
        }

        // GET: Surveys/Details/5
        public IActionResult Details(int id)
        {

            var survey = surveyService.GetSurveysById(id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // GET: Surveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Detail,numberOfConfirmation,numberOfYes,numberOfNo,CreatorId,Deadline,CreationDate")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                surveyService.AddSurvey(survey);

                int userId = (Convert.ToInt32(User.Identity.Name));
                var UserInfo = userService.GetUserById(userId);

                var users = userService.GetUsers();



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
                foreach (var item in users)
                {
                    if (item.MailConfirmation == true)
                    {
                        objeto_mail.To.Add(new MailAddress(item.Email));
                    }
                    
                }
                objeto_mail.Subject = "Yeni Anket";
                objeto_mail.Body =$"\n{survey.Title}\n" +
                    $"{survey.Detail}\n\n" +
                    $"Gerekli onay sayısı:{survey.numberOfConfirmation}\n" +
                    $"Anket kapanış tarihi: {survey.Deadline}\n\n"+
                    $"Anketi oluşturan : {UserInfo.Name} {UserInfo.LastName} ";


                client.Send(objeto_mail);


                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public IActionResult Edit(int id)
        {

            var survey = surveyService.GetSurveysById(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Title,Detail,numberOfConfirmation,numberOfYes,numberOfNo,CreatorId,Deadline,CreationDate")] Survey survey)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    surveyService.UpdateSurvey(survey);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json("Anket Güncellendi");
            }
            return View(survey);
        }

        // GET: Surveys/Delete/5
        public IActionResult Delete(int id)
        {
            var survey = surveyService.GetSurveysById(id);
            if (survey == null)
            {
                return NotFound();
            }

            return Json(survey.Title);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = surveyService.GetSurveysById(id);
            surveyService.deleteSurvey(survey);
            return Json("OK");
        }

        private bool SurveyExists(int id)
        {
            return surveyService.Exists(id);
            
        }



        public IActionResult DownloadExcelDocument()
        {

            var Survey = surveyService.GetSurveys();

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Rapor.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Survey");
                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "Başlık";
                    worksheet.Cell(1, 3).Value = "Detay";
                    worksheet.Cell(1, 4).Value = "Gerkeli Onay Sayısı";
                    worksheet.Cell(1, 5).Value = "Verilen Onay Sayısı";
                    worksheet.Cell(1, 6).Value = "Verilen Red Sayısı";
                    worksheet.Cell(1, 7).Value = "Toplam Oylayan Kişi Sayısı";
                    worksheet.Cell(1, 8).Value = "Deadline";
                     worksheet.Cell(1, 9).Value = "Anketin Oluşturulduğu Tarih";
                    worksheet.Cell(1, 10).Value = "Anketi Durumu";

                    int index = 1;
                    foreach (var item in Survey)
                    {
                        worksheet.Cell(index + 1, 1).Value = item.Id;
                        worksheet.Cell(index + 1, 2).Value = item.Title;
                        worksheet.Cell(index + 1, 3).Value = item.Detail;
                        worksheet.Cell(index + 1, 4).Value = item.numberOfConfirmation;
                        worksheet.Cell(index + 1, 5).Value = item.numberOfYes;
                        worksheet.Cell(index + 1, 6).Value = item.numberOfNo;
                        worksheet.Cell(index + 1, 7).Value = (item.numberOfNo + item.numberOfYes);
                        worksheet.Cell(index + 1, 8).Value = item.Deadline;
                        worksheet.Cell(index + 1, 9).Value = item.CreationDate;
                        if ((item.numberOfYes == item.numberOfConfirmation) || (DateTime.Now > item.Deadline))
                        {
                            if ((item.numberOfYes == item.numberOfConfirmation))
                            {
                                worksheet.Cell(index + 1, 10).Value = "Kabul edilmiştir";
                            }
                            else
                            {
                                worksheet.Cell(index + 1, 10).Value = "Kabul edilmemiştir";
                            }
                        }
                        else
                        {
                            worksheet.Cell(index + 1, 10).Value = "Anket henüz devam etmektedir";
                        }

                        index++;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
    }
}
