using System.ComponentModel.DataAnnotations;

namespace ItAcademy.ViewModels
{
    public class UpdateVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bu xana boş ola bilməz!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Role { get; set; }

       
    }
}
