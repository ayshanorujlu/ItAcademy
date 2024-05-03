using System.ComponentModel.DataAnnotations;

namespace ItAcademy.ViewModels
{
    public class ResetPasswordVM
    {
        public string? Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu  xana boş ola bilməz!!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
       
    }
}
