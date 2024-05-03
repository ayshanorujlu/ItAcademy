using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItAcademy.Models
{
    public class Courses
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public int Mark { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string CoursesDuration { get; set; }
        public bool IsDeactive { get; set; }
        public List<Students> Students { get; set; }
        public List<Teachers> Teachers { get; set; }
        public List<Groups> Groups { get; set; }
        

    }
}
