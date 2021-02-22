using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int numberOfConfirmation { get; set; }
        public int numberOfYes { get; set; }
        public int numberOfNo { get; set; }

        public int CreatorId { get; set; }

        public DateTime Deadline { get; set; }

        public List<Response> Responses { get; set; }
    }
}
