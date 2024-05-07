using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class SocietyViewModel
    {
        [Required]
        public string SocietyType { get; set; }
        [Key,Required]
        public string SocietyName { get; set; }
        [Required]
        public string SocietyIntro { get; set; }
        [Required]
        public string President { get; set; }
        [Required]
        public string Mentor { get; set; }
        [Required]
        public string GPA { get; set; }
        [Required]
        public string Semester { get; set; }
    }
}
