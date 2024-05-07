using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class TempFeedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } // Changed from 'content' to 'Content'
    }
}
