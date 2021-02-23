using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Models
{
    public class Survey
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Lütfen bir başlık girin")]
        [Display(Name = "Anket Başlığı")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Lütfen bir açıklama girin")]
        [Display(Name = "Anket Açıklaması")]
        public string Detail { get; set; }

        [Required(ErrorMessage = "Lütfen kabul için geren onay sayısı giriniz")]
        [Display(Name = "Gereken Onay Sayısı")]
        public int numberOfConfirmation { get; set; }

        [Display(Name = "Verilen Onay Sayısı")]
        public int numberOfYes { get; set; }

        [Display(Name = "Verilen Red Sayısı")]
        public int numberOfNo { get; set; }

        [Display(Name = "Anket Oluşturan Kişi Id")]
        public int CreatorId { get; set; }

        [Required(ErrorMessage = "Lütfen anketin oylanabileceği son tarihi girin")]
        [Display(Name = "Son Tarih")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreationDate { get; set; }

        public List<Response> Responses { get; set; }
    }
}
