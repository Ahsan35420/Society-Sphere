using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class Login
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        public string Password { get; set; }
    }
}
