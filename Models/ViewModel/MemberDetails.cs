using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class MemberDetails
    {
        [Required]
        public int Name { get; set; }
        [Required]
        public string RollNo { get; set; }
        [Required]
        public string Gpa { get; set; }
      
        public string SocietyName { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
