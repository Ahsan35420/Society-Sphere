using System;
using System.ComponentModel.DataAnnotations;

namespace SocietySphere.Models.ViewModel
{
    public class Loggedin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        public string SocietyName { get; set; }
    }
}
