using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Models
{
    public class Response
    {
        public int Id { get; set; }

        public string Answer { get; set; }
        public string Note { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Survey Survey { get; set; }
        public int SurveyId { get; set; }

    }
}
