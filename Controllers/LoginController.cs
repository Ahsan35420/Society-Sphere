using Microsoft.AspNetCore.Mvc;
using SocietySphere.Data;
using SocietySphere.Models.ViewModel;
using System;
using System.Linq;

namespace SocietySphere.Controllers
{
    public class LoginController : Controller
    {
        private readonly SocietyDBContext _dbContext;

        public LoginController(SocietyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login(Login login)
        {
            var username = login.Username;
            var password = login.Password;

            var admin = _dbContext.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            var organizer = _dbContext.Organizers.FirstOrDefault(a => a.Username == username && a.Password == password);
            var member = _dbContext.Members.FirstOrDefault(a => a.Username == username && a.Password == password);
            var leader = _dbContext.Leaders.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (admin != null)
            {
                SaveLoggedInUser(username, "Admin", "Admin Society"); // Provide a default society name for admin
                return RedirectToAction("Home", "Admin");
            }
            else if (organizer != null)
            {
                SaveLoggedInUser(username, "Organizer", organizer.SocietyName);
                return RedirectToAction("Home", "Organizer");
            }
            else if (member != null)
            {
                SaveLoggedInUser(username, "Member", member.SocietyName);
                return RedirectToAction("Home", "Member");
            }
            else if (leader != null)
            {
                SaveLoggedInUser(username, "Leader", leader.SocietyName);
                return RedirectToAction("Home", "Leader");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("Login");
            }
        }

        private void SaveLoggedInUser(string username, string userType, string societyName)
        {
            var loggedInUser = new Loggedin
            {
                Username = username,
                UserType = userType,
                SocietyName = societyName,
                LoginTime = DateTime.Now
            };
            _dbContext.loggedins.Add(loggedInUser);
            _dbContext.SaveChanges();
        }
    }
}
