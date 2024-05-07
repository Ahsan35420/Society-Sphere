using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class TempAnncs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
