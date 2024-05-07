using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietySphere.Data;
using SocietySphere.Models.ViewModel;

namespace SocietySphere.Controllers
{
    public class OrganizerController : Controller
    {
        private readonly SocietyDBContext _dbContext;

        public OrganizerController(SocietyDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Home(string name)
        {
            try
            {
                // Get all rows from the loggedin table
                var loggedInUsers = _dbContext.loggedins.ToList();

                // Remove all rows from the loggedin table
                _dbContext.loggedins.RemoveRange(loggedInUsers);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log and handle the exception gracefully
                Console.WriteLine($"Error logging out: {ex.Message}");
            }

            // Redirect the user to the login page after logout
            return RedirectToAction("Login", "Login");
        }

        public IActionResult Organize()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddAnn()
        {
            // Retrieve societies from the database
            var societies = _dbContext.Societies.Select(s => s.SocietyName).ToList();
            ViewBag.Societies = societies ?? new List<string>();

            return View(new TempAnncs()); // Pass an empty Announcement model to the view
        }

        [HttpPost]
        public IActionResult AddAnn(TempAnncs temp)
        {
            if (ModelState.IsValid)
            {
                var announcement = new Announcement();
                try
                {
                    // Retrieve society name and user type from all loggedin users
                    var loggedInUsers = _dbContext.loggedins.ToList();
                    if (loggedInUsers.Any())
                    {

                        announcement.Content = temp.Content;
                        // For simplicity, let's assume all logged in users belong to the same society and have the same user type
                        var firstUser = loggedInUsers.First();
                        announcement.SocietyName = firstUser.SocietyName;
                        announcement.UserType = firstUser.UserType;
                    }

                    // Add the announcement to the database
                    _dbContext.Announcements.Add(announcement);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Announcement", "Leader");
                }
                catch (Exception ex)
                {
                    // Log and handle the exception
                    Console.WriteLine($"Error adding announcement: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the announcement.");
                }
            }

            // If ModelState is not valid or there's an exception, return the view with the announcement model
            var societies = _dbContext.Societies.Select(s => s.SocietyName).ToList();
            ViewBag.Societies = societies ?? new List<string>();
            return View(temp);
        }

    }
}
