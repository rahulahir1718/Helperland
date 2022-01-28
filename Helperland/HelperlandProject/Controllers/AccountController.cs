using Microsoft.AspNetCore.Mvc;

namespace HelperlandProject.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return PartialView();
        }

        public IActionResult Register()
        {
            return View();
        }

        public ActionResult ForgotPassword() 
        {
            return PartialView();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
