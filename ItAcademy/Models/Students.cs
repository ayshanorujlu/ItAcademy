using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace ItAcademy.Models
{
    public class Students
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
        public int? Mobil { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        
        //[Required(ErrorMessage = "Bu xana boş ola bilməz!")]

        public DateTime? Birthday { get; set; }
        

        public string GetFormattedBirthday()
        {
            // "ToShortDateString()" metodu, tarihi kısa tarih biçiminde döndürür (örn. "20.07.1993")
            return Birthday?.ToString("dd:mm:yyyy");
        }

        [NotMapped]
        public IFormFile Photo { get; set; }

        public bool IsDeactive { get; set; }
        public Courses Courses { get; set; }
        public int CoursesId { get; set; }

        public List<GroupStudent> GroupStudent { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        public int Payment { get; set; }
        

    }
}
