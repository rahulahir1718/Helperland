using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using HelperlandProject.Models.ViewModels.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HelperlandProject.Controllers
{
    //only customers can access methods of this controller
    [Authorize(Roles = "1")]
    public class CustomerController : Controller
    {
        private readonly HelperlandContext helperlandContext;
        public CustomerController(HelperlandContext _helperlandContext)
        {
            helperlandContext = _helperlandContext;
        }

        public IActionResult ServiceRequest() 
        {
            //return partialview with all the requests which are completed and cancelled yet
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Include(x=>x.ServiceProvider).ThenInclude(x=>x.RatingRatingToNavigations).Where(x=>x.UserId==id && (x.Status == Constants.SERVICE_PENDING || x.Status == Constants.SERVICE_ACCEPTED)).ToList();
            return View(serviceRequests);
        }

        [HttpGet]
        public PartialViewResult ServiceRequestDetail(int id)
        {
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x=>x.ServiceRequestAddresses).Include(x=>x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == id);
            //returns partialview with service detail
            return PartialView(serviceRequest);
        }

        [HttpGet]
        public PartialViewResult RescheduleService(int id)
        {
            RescheduleServiceViewModel model = new RescheduleServiceViewModel();
            model.ServiceRequestId = id;
            model.ServiceDate = helperlandContext.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == id).ServiceStartDate;
            model.ServiceTime = helperlandContext.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == id).ServiceStartDate.ToString("HH:mm:ss");
            //return partialview for service rescheduling
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult RescheduleService(RescheduleServiceViewModel model)
        {
            var isConflict = false;
            //fetch service details from the database with given service request id
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x=>x.ServiceProvider).FirstOrDefault(x => x.ServiceRequestId == model.ServiceRequestId);
            var day = model.ServiceDate.ToString("dd-MM-yyyy");
            var actual = day + " " + model.ServiceTime;
            DateTime newStartDate = DateTime.Parse(actual);

            //if service is not accepted by any service provider yet then reschedule the request
            if (serviceRequest.ServiceProvider == null)
            {
                serviceRequest.ServiceStartDate = newStartDate;
                serviceRequest.ModifiedDate = DateTime.Now;
                helperlandContext.ServiceRequests.Update(serviceRequest);
                helperlandContext.SaveChanges();
                //return and show success message to customer
                var message = "Reschedule successfully..";
                ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                return PartialView(model);
            }
            //otherwise check time conflicts
            else
            {
                var newStartTime = newStartDate;
                var newEndTime = newStartTime.AddHours(serviceRequest.ServiceHours);
                //fetch all the request from database which are not completed yet with same service provider id 
                IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == serviceRequest.ServiceProviderId && x.Status == Constants.SERVICE_ACCEPTED && x.ServiceRequestId!=serviceRequest.ServiceRequestId).ToList();
                foreach (var request in serviceRequests)
                {
                    var oldStartTime = request.ServiceStartDate;
                    var oldEndTime = oldStartTime.AddHours(request.ServiceHours);
                    isConflict = false;
                    //check time conflicts
                    if ((request.ServiceStartDate == newStartDate) || (((newStartTime > oldStartTime) && (newStartTime < oldEndTime)) || ((newEndTime > oldStartTime) && (newEndTime < oldEndTime))))
                    {
                        isConflict = true;
                        break;
                    }
                }
                //if time conflicts then show error message to customer othewise go ahead and reschedule request
                if (isConflict)
                {
                    var message = "Another service request has been assigned to the service provider on " + newStartDate.Date.ToString() + " from " + newStartTime.TimeOfDay.ToString() + " to" + newEndTime.TimeOfDay.ToString() + ". Either choose another date or pick up a different time slot.";
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return PartialView(model);
                }
                else
                {
                    serviceRequest.ServiceStartDate = newStartDate;
                    serviceRequest.ModifiedDate = DateTime.Now;
                    helperlandContext.ServiceRequests.Update(serviceRequest);
                    helperlandContext.SaveChanges();
                    //send email to the service provider about service reschedule
                    List<string> emailList = new List<string>();
                    emailList.Add(serviceRequest.ServiceProvider.Email);
                    //helperlandContext.Users.Where(user => user.UserId == serviceRequest.ServiceProviderId).Select(user => user.Email).ToList();
                    string subject = "Request Rescheduled!";
                    string body = "Service Request " + serviceRequest.ServiceRequestId + " has been rescheduled by customer. New date and time are " + newStartDate.ToString();
                    EmailManager.SendEmail(emailList, subject, body);
                    //return and show success message to customer
                    var message = "Reschedule successfully..";
                    ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return PartialView(model);
                }
            }
        }

        [HttpGet]
        public PartialViewResult CancelServiceRequest()
        {
            //return partialview for service cancellation
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult CancelServiceRequest(int id, string comment)
        {
            //fetch specific service from database
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x=>x.ServiceProvider).FirstOrDefault(x => x.ServiceRequestId == id);
            //set message for cancel request
            serviceRequest.Comments = comment;
            //set status to cancelled service=4
            serviceRequest.Status = Constants.SERVICE_CANCELLED;
            serviceRequest.ModifiedDate = DateTime.Now;
            helperlandContext.ServiceRequests.Update(serviceRequest);
            helperlandContext.SaveChanges();
            //if request was accepted by any service provider then send email
            if (serviceRequest.ServiceProvider != null)
            {
                List<string> emailList = new List<string>();
                emailList.Add(serviceRequest.ServiceProvider.Email);
                //helperlandContext.Users.Where(user => user.UserId == serviceRequest.ServiceProviderId).Select(user => user.Email).ToList();
                string subject = "Request Cancelled!";
                string body = "Service Request " + serviceRequest.ServiceRequestId + " has been cancelled by customer.";
                EmailManager.SendEmail(emailList, subject, body);
            }
            //return and show success message to customer
            var message = "Request cancelled successfully..";
            ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            return PartialView();
        }

        public IActionResult ServiceHistory() 
        {
            //return partialview with all the requests which are completed,cancelled or refunded.
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Include(x=>x.ServiceProvider).ThenInclude(x=>x.RatingRatingToNavigations).Where(x=>x.UserId==id &&(x.Status != Constants.SERVICE_PENDING && x.Status != Constants.SERVICE_ACCEPTED)).ToList();
            return View(serviceRequests);
        }

        [HttpGet]
        public JsonResult EditRating(int id)
        {
            Rating rating = helperlandContext.Ratings.Include(x=>x.RatingToNavigation).FirstOrDefault(x => x.ServiceRequestId == id);
            return Json(rating);
        }

        [HttpPost]
        public JsonResult EditRating([FromBody]Rating model)
        {
            Rating rating = helperlandContext.Ratings.FirstOrDefault(x => x.RatingId == model.RatingId);
            rating.RatingDate = DateTime.Now;
            rating.Ratings = model.Ratings;
            rating.OnTimeArrival = model.OnTimeArrival;
            rating.Friendly = model.Friendly;
            rating.QualityOfService = model.QualityOfService;
            rating.Comments = model.Comments;
            helperlandContext.Ratings.Update(rating);
            helperlandContext.SaveChanges();
            return Json(" ");
        }

        public IActionResult FavouriteProviders()
        {
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavouriteProvidersViewModel model = new FavouriteProvidersViewModel();
            model.ServiceProviders= helperlandContext.ServiceRequests.Include(x => x.ServiceProvider).ThenInclude(x => x.RatingRatingToNavigations).Where(x => x.UserId == id && x.Status == Constants.SERVICE_COMPLETED).Select(x => x.ServiceProvider).Distinct().AsEnumerable();
            model.FavouriteSpIds = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == id && x.IsFavorite==true).Select(x => x.TargetUserId).Distinct().ToList();
            model.BlockedSpIds = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == id && x.IsBlocked == true).Select(x => x.TargetUserId).Distinct().ToList();
            return View(model);
        }

        public JsonResult MarkFavourite(int spId)
        {
            int customerId= Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked fSP = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x => x.UserId == customerId && x.TargetUserId == spId);
            if (fSP == null)
            {
                fSP = new FavoriteAndBlocked();
                fSP.UserId = customerId;
                fSP.TargetUserId = spId;
                fSP.IsFavorite = true;
                helperlandContext.FavoriteAndBlockeds.Add(fSP);
                helperlandContext.SaveChanges();
            }
            else 
            {
                fSP.IsFavorite = true;
                helperlandContext.FavoriteAndBlockeds.Update(fSP);
                helperlandContext.SaveChanges();
            }
            return Json("ok");
        }

        public JsonResult MarkUnfavourite(int spId)
        {
            int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked fSP = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x => x.UserId == customerId && x.TargetUserId == spId && x.IsFavorite==true);
            fSP.IsFavorite = false;
            helperlandContext.FavoriteAndBlockeds.Update(fSP);
            helperlandContext.SaveChanges();
            return Json("ok");
        }

        public JsonResult MarkBlocked(int spId) 
        {
            int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked bSP = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x => x.UserId == customerId && x.TargetUserId == spId);
            if (bSP == null)
            {
                bSP = new FavoriteAndBlocked();
                bSP.UserId = customerId;
                bSP.TargetUserId = spId;
                bSP.IsBlocked = true;
                helperlandContext.FavoriteAndBlockeds.Add(bSP);
                helperlandContext.SaveChanges();
            }
            else 
            {
                bSP.IsBlocked = true;
                helperlandContext.FavoriteAndBlockeds.Update(bSP);
                helperlandContext.SaveChanges();
            }
            return Json("ok");
        }

        public JsonResult MarkUnBlocked(int spId)
        {
            int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked fSP = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x => x.UserId == customerId && x.TargetUserId == spId && x.IsBlocked == true);
            fSP.IsBlocked = false;
            helperlandContext.FavoriteAndBlockeds.Update(fSP);
            helperlandContext.SaveChanges();
            return Json("ok");
        }
        public IActionResult MyAccount() 
        {
            //returns view for My Account
            return View();
        }

        [HttpGet]
        public PartialViewResult MyDetails()
        {
            //fetch user details from the session
            string currentUser =HttpContext.Session.GetString("CurrentUser");
            User user = JsonConvert.DeserializeObject<User>(currentUser);
            MyDetailsViewModel model = new MyDetailsViewModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.Mobile = user.Mobile;
            model.LanguageId = user.LanguageId;
            if (user.DateOfBirth != null) {
                var DOB=user.DateOfBirth.Value.ToString("dd/MMMM/yyyy").Split("-");
                model.BirthDay = DOB[0];
                model.BirthMonth = DOB[1];
                model.BirthYear = DOB[2];
            }
            //return partialview with details
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult MyDetails(MyDetailsViewModel model) 
        {
            //fetch user details from session
            string currentUser = HttpContext.Session.GetString("CurrentUser");
            User user = JsonConvert.DeserializeObject<User>(currentUser);
            string DOB = model.BirthDay + "-" + model.BirthMonth + "-" + model.BirthYear;
            user.DateOfBirth = DateTime.Parse(DOB);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Mobile = model.Mobile;
            user.LanguageId = model.LanguageId;
            //update details in the database
            helperlandContext.Users.Update(user);
            helperlandContext.SaveChanges();
            //update user details in session
            HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
            var message = "Details Updated successfully..";
            ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult MyAddresses() 
        {
            //return partialview with all the user addresses
            int id = Int16.Parse(User.Claims.FirstOrDefault(x=>x.Type=="userId").Value);
            IEnumerable<UserAddress> userAddresses = helperlandContext.UserAddresses.Where(x=>x.UserId==id).ToList();
            return PartialView(userAddresses);
        }

        [HttpGet]
        public PartialViewResult EditAddress(int? id)
        {
            //return partialview with details of one specific address
            if (id != null)
            {
                UserAddress userAddress = helperlandContext.UserAddresses.FirstOrDefault(x => x.AddressId == id);
                EditAddressViewModel model = new EditAddressViewModel();
                model.AddressId = userAddress.AddressId;
                model.StreetName = userAddress.AddressLine1;
                model.HouseNumber = userAddress.AddressLine2;
                model.PostalCode = userAddress.PostalCode;
                model.City = userAddress.City;
                model.PhoneNumber = userAddress.Mobile;
                return PartialView(model);
            }
            else 
            {
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult EditAddress(EditAddressViewModel model)
        {
            //check validations and update result in database or add new address into database 
            if (ModelState.IsValid)
            {
                if (model.AddressId != null)
                {
                    UserAddress userAddress = helperlandContext.UserAddresses.FirstOrDefault(x => x.AddressId == model.AddressId);
                    userAddress.AddressLine1 = model.StreetName;
                    userAddress.AddressLine2 = model.HouseNumber;
                    userAddress.PostalCode = model.PostalCode;
                    userAddress.City = model.City;
                    userAddress.Mobile = model.PhoneNumber;
                    helperlandContext.UserAddresses.Update(userAddress);
                    helperlandContext.SaveChanges();
                    return PartialView();
                }
                else 
                {
                    UserAddress userAddress = new UserAddress();
                    userAddress.UserId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                    userAddress.AddressLine1 = model.StreetName;
                    userAddress.AddressLine2 = model.HouseNumber;
                    userAddress.PostalCode = model.PostalCode;
                    userAddress.City = model.City;
                    userAddress.Mobile = model.PhoneNumber;
                    helperlandContext.UserAddresses.Add(userAddress);
                    helperlandContext.SaveChanges();
                    return PartialView();
                }
            }
            else
            {
                return PartialView(model);
            }
        }

        [HttpPost]
        public JsonResult DeleteAddress(int id)
        {
            //delete specific address from database
            UserAddress userAddress = helperlandContext.UserAddresses.FirstOrDefault(x=>x.AddressId==id);
            helperlandContext.UserAddresses.Remove(userAddress);
            helperlandContext.SaveChanges();
            return Json("done");
        }

        [HttpPost]
        public PartialViewResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //fetch user details from session
                string currentUser = HttpContext.Session.GetString("CurrentUser");
                User user = JsonConvert.DeserializeObject<User>(currentUser);
                //update user's password into the database if current password is correctly entered,otherwise return partialview with error message
                if (user.Password.Equals(Constants.EncryptString(model.CurrentPassword)))
                {
                    user.Password = Constants.EncryptString(model.ConfirmPassword);
                    helperlandContext.Users.Update(user);
                    helperlandContext.SaveChanges();
                    HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
                    model = null;
                    var message = "Password changed successfully..";
                    ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return PartialView(model);
                }
                else
                {
                    var message = "Incorrect old password";
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return PartialView(model);
                }
            }
            else 
            {
                return PartialView(model);
            }
        }
    }
}
