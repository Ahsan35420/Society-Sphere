using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietySphere.Data;
using SocietySphere.Models.ViewModel;

namespace SocietySphere.Controllers
{
    public class MemberController : Controller
    {
        private readonly SocietyDBContext _dbContext;

        public MemberController(SocietyDBContext dbContext)
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
        public IActionResult JoinS() 
        {
            return View();
        }

        public IActionResult Participate()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FeedBack()
        {
            // Retrieve societies from the database
            var societies = _dbContext.Societies.Select(s => s.SocietyName).ToList();
            ViewBag.Societies = societies ?? new List<string>();

            return View(new TempFeedback()); // Pass an empty Announcement model to the view
        }

        [HttpPost]
        public IActionResult FeedBack(TempFeedback temp)
        {
            if (ModelState.IsValid)
            {
                var feedback = new Feedback();
                try
                {
                    // Retrieve society name and user type from all loggedin users
                    var loggedInUsers = _dbContext.loggedins.ToList();
                    if (loggedInUsers.Any())
                    {

                        feedback.Content = temp.Content;
                        // For simplicity, let's assume all logged in users belong to the same society and have the same user type
                        var firstUser = loggedInUsers.First();
                        feedback.SocietyName = firstUser.SocietyName;
                        feedback.UserType = firstUser.UserType;
                        feedback.Username = firstUser.Username;
                    }

                    // Add the announcement to the database
                    _dbContext.feedback.Add(feedback);
                    _dbContext.SaveChanges();

                    return RedirectToAction("FeedBack", "Member");
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

        public IActionResult ViewFeed()
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                var societyName = loggedInUser.SocietyName;
                var feedbacks = _dbContext.feedback.Where(f => f.SocietyName == societyName && f.Username == loggedInUser.Username).ToList();
                return View(feedbacks);
            }
            else
            {
                // Handle the case where no user is logged in
                return View();
            }
        }
        public IActionResult ViewAnnouncement()
        {
            // Get the society name from the loggedin table
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser == null)
            {
                // No logged in user found, return an empty list of announcements
                return View(new List<Announcement>());
            }

            // Retrieve announcements for the logged in user's society
            var announcements = _dbContext.Announcements
                                        .Where(a => a.SocietyName == loggedInUser.SocietyName)
                                        .ToList();

            return View(announcements);
        }
    }
}
