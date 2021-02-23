using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConfirmationProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen isim girin")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Lütfen soyadınızı girin")]
        public string LastName { get; set; }

        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "Lütfen email adresinizi girin")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        
        [Required(ErrorMessage = "Lütfen kullanıcı adı girin")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen şifre girin")]
        public string Password { get; set; }

        [Display(Name = "Telefon Numarası")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Rol")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Display(Name = "Cinsiyet")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        public IList<Response> Responses { get; set; }
    }
}
