using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ItAcademy.Models
{
    public class Employers
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public long? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Salary { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public bool IsDeactive { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public DateTime? Birthday { get; set; }
        public string GetFormattedBirthday()
        {
            // "ToShortDateString()" metodu, tarihi kısa tarih biçiminde döndürür (örn. "20.07.1993")
            return Birthday?.ToShortDateString();
        }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public Positions Positions { get; set; }
        public int PositionsId { get; set; }
       

    }
}
