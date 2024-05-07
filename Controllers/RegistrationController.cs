using Microsoft.AspNetCore.Mvc;

namespace SocietySphere.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }
    }
}
