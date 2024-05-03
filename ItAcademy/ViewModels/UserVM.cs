using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ItAcademy.ViewModels
{
    public class UserVM
    {
        

        public string Id { get; set; }
        public string Name { get; set; }
      

        public string Username { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }


        public bool IsDeactive { get; set; }
        public string Role { get; set; }
      
    }
}
