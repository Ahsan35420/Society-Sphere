using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietySphere.Data;
using SocietySphere.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocietySphere.Controllers
{
    public class GuestController : Controller
    {
        private readonly SocietyDBContext _dbContext;

        public GuestController(SocietyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult RequestForJoin(TempMember member)
        {
            // Check if the username already exists in the Members table
            if (_dbContext.Members.Any(m => m.Username == member.Username))
            {
                ModelState.AddModelError("Username", "Username already exists. Please choose a different username.");
            }

            // Password syntax validation
            if (!IsValidPassword(member.Password))
            {
                ModelState.AddModelError("Password", "Password must be 8-12 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Add member to the database
                    _dbContext.tmpmembers.Add(member);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index", "Guest");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error adding member: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the member.");
                }
            }

            // Re-populate ViewBag.Societies if there are validation errors
            var societies = _dbContext.Societies.Select(s => s.SocietyName).ToList();
            ViewBag.Societies = societies ?? new List<string>();
            return View(member);
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

        public IActionResult AllSocieties()
        {
            var societies = _dbContext.Societies.ToList();
            return View(societies);
        }
    }
}
