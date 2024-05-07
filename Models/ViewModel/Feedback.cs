using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string SocietyName { get; set; }

        [Required]
        public string Content { get; set; } // Changed from 'content' to 'Content'

        [Required]
        public string UserType { get; set; }
    }
}

