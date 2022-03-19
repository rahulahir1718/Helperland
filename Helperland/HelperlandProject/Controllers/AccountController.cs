using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using HelperlandProject.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace HelperlandProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly HelperlandContext helperlandContext;
        public AccountController(HelperlandContext _helperlandContext)
        {
            helperlandContext = _helperlandContext;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            string message;
            if (ModelState.IsValid)
            {
                User user=helperlandContext.Users.Where(user => user.Email.Equals(model.Email) && user.Password.Equals(model.Password)).FirstOrDefault();
                if (user != null)
                {
                    if (user.IsApproved)
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                        identity.AddClaim(new Claim("userId", user.UserId.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name,user.FirstName+" "+user.LastName));
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.UserTypeId.ToString()));
                        
                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                            IsPersistent = model.RememberMe,
                        };

                        HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal ,authProperties);
                        string returnUrl=(string)TempData["returnUrl"];

                        if (returnUrl != null)
                        {
                            return Json("returnUrl="+returnUrl);
                        }
                        else 
                        {
                            switch (user.UserTypeId)
                            {
                                case Constants.CUSTOMER : return Json("returnUrl=/customer/servicerequest");
                                case Constants.SERVICE_PROVIDER : return Json("returnUrl=/serviceprovider/newservicerequest");
                                case Constants.ADMIN : return Json("returnUrl=/admin/servicerequests");
                                default: return Json("returnUrl=/home/index");
                            }
                        }

                    }
                    else 
                    {
                        message = "Your account is yet to be approve by admin..Try again after some time!!";
                        ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                        return View(model);
                    }
                }
                else 
                {
                    message = "Invalid username or password";
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isEmailAlreadyExists = helperlandContext.Users.Any(user=>user.Email==model.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>User with this email already exists<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View(model);
                }
                else
                {
                    User user = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Password = model.Password,
                        UserTypeId = Constants.CUSTOMER,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsApproved=true,
                        IsActive=true,
                        IsRegisteredUser=true
                    };

                    helperlandContext.Users.Add(user);
                    helperlandContext.SaveChanges();
                    return RedirectToAction("index", "home", new { loginPopUp=true});
                }
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult BecomeAProvider()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BecomeAProvider(RegisterUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var isEmailAlreadyExists = helperlandContext.Users.Any(user => user.Email == model.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>Service Provider with this email already exists<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View(model);
                }
                else
                {
                    User user = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Password = model.Password,
                        UserTypeId = Constants.SERVICE_PROVIDER,
                        IsRegisteredUser = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };

                    helperlandContext.Users.Add(user);
                    helperlandContext.SaveChanges();
                    return RedirectToAction("index", "home", new { loginPopUp = true });
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword() 
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isEmailExists = helperlandContext.Users.Any(user => user.Email == model.Email);
                if (isEmailExists)
                {
                    var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { email = model.Email}, "https") + "'>Reset Password</a>";

                    string subject = "Reset Password";
                    string body = "<b>Please find the Password Reset Link. </b><br/><br/>" + lnkHref;
                    List<string> toList = new List<string>();
                    toList.Add(model.Email);
                    EmailManager.SendEmail(toList,subject,body);
                    ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>An email has been sent to your account. <b>Click on the link in received email to reset the password.</b><button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View();
                }
                else
                {
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>Email address not found..make sure that you have entered a correct one<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        } 

        [HttpGet]
        public IActionResult ResetPassword(string Email)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Email = Email;
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = helperlandContext.Users.Where(user => user.Email.Equals(model.Email)).FirstOrDefault();
                user.Password = model.Password;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = user.UserId;
                helperlandContext.Users.Update(user);
                helperlandContext.SaveChanges();
                return RedirectToAction("index", "home", new { loginPopUp = true });
            }
            else
            {
                return View(model);
            }
            
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index","home");
        }
    }
}

