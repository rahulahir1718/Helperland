using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using HelperlandProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace HelperlandProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly HelperlandContext helperlandContext;

        public HomeController(ILogger<HomeController> _logger, HelperlandContext _helperlandContext)
        {
            logger = _logger;
            helperlandContext = _helperlandContext;
        }

        public IActionResult Index(Boolean loginPopUp,string? ReturnUrl)
        {
            ViewBag.loginPopUp = loginPopUp;
            TempData["returnUrl"] = ReturnUrl;
            return View();
        }

        public IActionResult Faqs()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult ContactUs(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userString = HttpContext.Session.GetString("CurrentUser");
                int createdBy=0;
                if (userString != null) 
                {
                    User user = JsonConvert.DeserializeObject<User>(userString);
                    createdBy = user.UserId;
                }
                string uniqueFileName = null;
                if (model.File != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ContactUsAttechment");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                ContactU contact = new()
                {
                    Name = model.FirstName + " " + model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    Message = model.Message,
                    Subject = model.Subject,
                    UploadFileName = model.File?.FileName,
                    CreatedOn = DateTime.Now,
                    FileName = uniqueFileName,
                    CreatedBy = createdBy
                };
                helperlandContext.ContactUs.Add(contact);
                helperlandContext.SaveChanges();
                return Json("Query submitted successfully..");
            }
            else
            {
                return Json(ModelState.Values);
            }
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}