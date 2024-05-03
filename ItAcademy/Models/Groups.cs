using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItAcademy.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public List<GroupStudent> GroupStudent { get; set; }
        //public List<TeacherGroup> TeacherGroups { get; set; }
        public bool IsDeactive { get; set; }

        public Courses Courses { get; set; }

        public int CoursesId { get; set; }
        
    }
}
