using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietySphere.Data;
using SocietySphere.Models;
using SocietySphere.Models.ViewModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocietySphere.Controllers
{
    public class AdminController : Controller
    {
        private readonly SocietyDBContext _context;

        public AdminController(SocietyDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Retrieve the username of the logged-in user
            var username = User.Identity.Name;

            // Find the logged-in user's row in the database
            var loggedInUser = _context.loggedins.FirstOrDefault(u => u.Username == username);

            if (loggedInUser != null)
            {
                // Remove the logged-in user's row from the database
                _context.loggedins.Remove(loggedInUser);
                _context.SaveChanges();
            }

            // Redirect the user to the login page
            return RedirectToAction("Login", "Login");
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            var members = _context.Members.ToList();
            ViewBag.Members = members;
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocietyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map data from view model to your entity class
                var society = new SocietyViewModel
                {
                    SocietyType = model.SocietyType,
                    SocietyName = model.SocietyName,
                    SocietyIntro = model.SocietyIntro,
                    President = model.President,
                    Mentor = model.Mentor,
                    GPA = model.GPA,
                    Semester = model.Semester
                };

                if (_context.Societies.Any(m => m.SocietyName == society.SocietyName))
                {
                    ModelState.AddModelError("SocietyName", "Society Name already exists. Please choose a different Society Name.");
                    return View(model); // Return the view with errors
                }

                // Add the society to the context and save changes to the database
                _context.Societies.Add(society);
                await _context.SaveChangesAsync();

                // Remove the selected member from the Member table
                var selectedMember = await _context.Members.FirstOrDefaultAsync(m => m.Username == model.President);
                if (selectedMember != null && double.TryParse(selectedMember.GPA, out double gpaValue) && gpaValue >= 3.0)
                {
                    // Add the selected member to the Leader table
                    var leader = new Leader
                    {
                        Username = selectedMember.Username,
                        Password = selectedMember.Password,
                        GPA = selectedMember.GPA, // Assign GPA value here
                        Semester = selectedMember.Semester,
                        SocietyName = selectedMember.SocietyName,
                                        // Add other properties as needed
                    };
                    _context.Leaders.Add(leader);
                    _context.Members.Remove(selectedMember); // Move this line here
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("GPA", "GPA is Less than 3.0");
                    return View(model); // Return the view with errors
                }

                // Redirect to a success page or return a success message
                return RedirectToAction("Home", "Admin");
            }

            // If model state is not valid, return to the same view with validation errors
            return View(model);
        }


        [HttpGet]
        public async Task<JsonResult> GetMemberDetails(string username)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Username == username);
            if (member != null)
            {
                var memberDetails = new { GPA = member.GPA, Semester = member.Semester };
                return Json(memberDetails);
            }
            return Json(null);
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
                var loggedInUsers = _context.loggedins.ToList();

                // Remove all rows from the loggedin table
                _context.loggedins.RemoveRange(loggedInUsers);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log and handle the exception gracefully
                Console.WriteLine($"Error logging out: {ex.Message}");
            }

            // Redirect the user to the login page after logout
            return RedirectToAction("Login", "Login");
        }


        public IActionResult FeedBack()
        {
            var feedback = _context.feedback.ToList();
            return View(feedback);
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Plans()
        {
            return View();
        }

        public IActionResult Members()
        {
            return View();
        }

        public IActionResult Ann()
        {
            var announcements = _context.Announcements.ToList();
            return View(announcements);
        }

    }
}
