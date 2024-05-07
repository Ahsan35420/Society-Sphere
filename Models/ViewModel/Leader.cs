using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class Leader
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string GPA { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string SocietyName { get; set; }
    }
}
