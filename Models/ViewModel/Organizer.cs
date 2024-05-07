using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class Organizer
    {
        [Required,Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string SocietyName { get; set; }

    }
}
