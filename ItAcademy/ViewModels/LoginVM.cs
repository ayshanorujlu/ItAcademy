using System.ComponentModel.DataAnnotations;

namespace ItAcademy.ViewModels
{
    public class LoginVM
    {
       
        [Required(ErrorMessage = "Buxana boş ola bilməz!!")]
        public string Username { get; set; }

        

        [Required(ErrorMessage = "Buxana boş ola bilməz!!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
       
        public bool IsRemember { get; set; }
    }
}
