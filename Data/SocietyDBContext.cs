using Microsoft.EntityFrameworkCore;
using SocietySphere.Models.ViewModel;
namespace SocietySphere.Data
{
    public class SocietyDBContext: DbContext
    { 
        public SocietyDBContext(DbContextOptions options): base(options) { 

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<SocietyViewModel> Societies { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Loggedin> loggedins { get; set; }

        public DbSet<TempMember> tmpmembers { get; set; }

        public DbSet<Feedback> feedback { get; set; }
    }
}
