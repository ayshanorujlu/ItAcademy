using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItAcademy.Models
{
    public class Positions
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Name { get; set; }
        public List<Employers> Employers { get; set; }
        //public List<Teachers> Teachers { get; set; }
        public bool IsDeactive { get; set; }
    }
}
