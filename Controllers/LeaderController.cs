using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietySphere.Data;
using SocietySphere.Models;
using SocietySphere.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocietySphere.Controllers
{
    public class LeaderController : Controller
    {
        private readonly SocietyDBContext _dbContext;

        public LeaderController(SocietyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AddOrganizer()
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                var societyName = loggedInUser.SocietyName;
                var memberUsernames = _dbContext.Members
                                            .Where(m => m.SocietyName == societyName)
                                            .Select(m => m.Username)
                                            .ToList();
                ViewBag.Members = memberUsernames ?? new List<string>();
                return View();
            }
            else
            {
                // Handle the case where no user is logged in
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddOrganizer(string username)
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                // Generate password for the organizer
                var password = GeneratePassword(username);

                // Add the organizer to the Organizers table
                var organizer = new Organizer
                {
                    Username = username,
                    Password = password,
                    SocietyName = loggedInUser.SocietyName,
   
                };
                _dbContext.Organizers.Add(organizer);
                _dbContext.SaveChanges();

                // Remove the organizer from the Members table
                var memberToRemove = _dbContext.Members.FirstOrDefault(m => m.Username == username && m.SocietyName == loggedInUser.SocietyName);
                if (memberToRemove != null)
                {
                    _dbContext.Members.Remove(memberToRemove);
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("AddOrganizer","Leader");
            }
            else
            {
                // Handle the case where no user is logged in
                return RedirectToAction("Index", "Leader");
            }
        }

        // Helper method to generate password
        private string GeneratePassword(string username)
        {
            // Generate password based on the specified format
            string password = "12345" + char.ToUpper(username[0]) + username.Substring(1, 3);
            return password + "*";
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


        [HttpGet]
        [HttpPost]
        public IActionResult AddMember()
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                var societyName = loggedInUser.SocietyName;
                var requests = _dbContext.tmpmembers.Where(m => m.SocietyName == societyName).ToList();
                return View(requests);
            }
            else
            {
                // Handle the case where no user is logged in
                return View();
            }
        }

        [HttpPost]
        public IActionResult AcceptRequest(string username)
        {
            var request = _dbContext.tmpmembers.FirstOrDefault(m => m.Username == username);
            if (request != null)
            {
                // Add the member to the Members table
                var member = new Member
                {
                    Username = request.Username,
                    Password = request.Password,
                    SocietyName = request.SocietyName,
                    GPA = request.GPA,
                    Semester = request.Semester
                };
                _dbContext.Members.Add(member);
                _dbContext.SaveChanges();

                // Remove the request from the TempMembers table
                _dbContext.tmpmembers.Remove(request);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Home","Leader");
        }

        [HttpPost]
        public IActionResult RejectRequest(string username)
        {
            var request = _dbContext.tmpmembers.FirstOrDefault(m => m.Username == username);
            if (request != null)
            {
                // Remove the request from the TempMembers table
                _dbContext.tmpmembers.Remove(request);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Home","Leader");
        }

        // Helper method to validate password syntax
        private bool IsValidPassword(string password)
        {
            // Implement your password syntax validation logic here
            // For simplicity, let's assume a valid password must be 8-12 characters long
            // and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.
            return !string.IsNullOrWhiteSpace(password) &&
                   password.Length >= 8 &&
                   password.Length <= 12 &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public IActionResult RemoveMember()
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                var societyName = loggedInUser.SocietyName;
                var memberUsernames = _dbContext.Members
                                            .Where(m => m.SocietyName == societyName)
                                            .Select(m => m.Username)
                                            .ToList();
                ViewBag.Members = memberUsernames ?? new List<string>();
                return View();
            }
            else
            {
                // Handle the case where no user is logged in
                return View();
            }
        }

        [HttpPost]
        public IActionResult RemoveMember(string username)
        {
            // Remove the organizer from the Members table
            var memberToRemove = _dbContext.Members.FirstOrDefault(m => m.Username == username);
            if (memberToRemove != null)
            {
               _dbContext.Members.Remove(memberToRemove);
               _dbContext.SaveChanges();
            }

            return RedirectToAction("Home","Leader");
           
        }


        public IActionResult ViewFeed()
        {
            var loggedInUser = _dbContext.loggedins.FirstOrDefault();
            if (loggedInUser != null)
            {
                var societyName = loggedInUser.SocietyName;
                var feedbacks = _dbContext.feedback.Where(f => f.SocietyName == societyName).ToList();
                return View(feedbacks);
            }
            else
            {
                // Handle the case where no user is logged in
                return View();
            }
        }

        public IActionResult FeedBack()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Announcement()
        {
            // Retrieve societies from the database
            var societies = _dbContext.Societies.Select(s => s.SocietyName).ToList();
            ViewBag.Societies = societies ?? new List<string>();

            return View(new TempAnncs()); // Pass an empty Announcement model to the view
        }

        [HttpPost]
        public IActionResult Announcement(TempAnncs temp)
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
