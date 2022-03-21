using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using HelperlandProject.Models.ViewModels.ServiceProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HelperlandProject.Controllers
{
    //only service providers can access methods of this controller
    [Authorize(Roles = "2")]
    public class ServiceProviderController : Controller
    {
        private readonly HelperlandContext helperlandContext;
        public ServiceProviderController(HelperlandContext _helperlandContext)
        {
            helperlandContext = _helperlandContext;
        }

        public IActionResult NewServiceRequest()
        {
            int spId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            //fetch all the block customers who have been blocked by logged in service provider
            IEnumerable<int> blockedCustomerList = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == spId && x.IsBlocked==true).Select(x => x.TargetUserId).Distinct().ToList();
            //fetch all the customers id who have blocked logged in service provider
            IEnumerable<int> blockedByCustomerList = helperlandContext.FavoriteAndBlockeds.Where(x => x.TargetUserId == spId && x.IsBlocked == true).Select(x => x.UserId).Distinct().ToList();
            //fetch list of service requests which are created by customers except blocked customers and blocked by customers
            IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Include(x=>x.User).ThenInclude(x=>x.UserAddresses.Where(x=>x.IsDefault==true)).Where(x => x.Status == Constants.SERVICE_PENDING && !blockedCustomerList.Any(a=>a==x.UserId) && !blockedByCustomerList.Any(a=>a==x.UserId)).ToList();
            //return view with list
            return View(serviceRequests);
        }

        [HttpGet]
        public PartialViewResult ServiceRequestDetail(int id)
        {
            //fetch all the details of perticular service request with given id
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x => x.User).Include(x => x.ServiceRequestAddresses).Include(x => x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == id);
            //store current record version of the request in session for concurrency control
            HttpContext.Session.SetString("CurrentServiceRecordVersion",serviceRequest.RecordVersion.Value.ToString());
            //return partialview with service request detail
            return PartialView(serviceRequest);
        }

        //this method handles different action calls like accept request,cancel request and complete request
        [HttpPost]
        public PartialViewResult ServiceRequestDetail(int requestId,string process)
        {
            int spId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            //this block handles cancel service action request
            if (process.Equals("Cancel"))
            {
                ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x => x.User).Include(x => x.ServiceRequestAddresses).Include(x => x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == requestId);
                serviceRequest.Status = Constants.SERVICE_PENDING;
                serviceRequest.ServiceProviderId = null;
                serviceRequest.ModifiedBy = spId;
                serviceRequest.ModifiedDate= DateTime.Now;
                helperlandContext.ServiceRequests.Update(serviceRequest);
                helperlandContext.SaveChanges();
                var message = "You have successfully cancellled the request.";
                ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                return PartialView(serviceRequest);
            }
            //this block handles complete service action request
            else if (process.Equals("Complete"))
            {
                ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x => x.User).Include(x => x.ServiceRequestAddresses).Include(x => x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == requestId);
                serviceRequest.Status = Constants.SERVICE_COMPLETED;
                serviceRequest.ModifiedBy = spId;
                serviceRequest.ModifiedDate = DateTime.Now;
                helperlandContext.ServiceRequests.Update(serviceRequest);
                helperlandContext.SaveChanges();
                Rating rating = new Rating();
                rating.ServiceRequestId = serviceRequest.ServiceRequestId;
                rating.RatingTo = spId;
                rating.RatingFrom = serviceRequest.UserId;
                rating.RatingDate = DateTime.Now;
                rating.OnTimeArrival = 0;
                rating.QualityOfService = 0;
                rating.Friendly = 0;
                rating.Ratings = 0;
                helperlandContext.Ratings.Add(rating);
                helperlandContext.SaveChanges();
                var message = "You have successfully completed the request.";
                ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                return PartialView(serviceRequest);
            }
            //this block handles accept service action request
            else
            {
                int conflictedServiceId = 0;
                Boolean isConflict = false;
                //get stored record version from session
                string recordVersion = HttpContext.Session.GetString("CurrentServiceRecordVersion");
                ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x => x.User).Include(x => x.ServiceRequestAddresses).Include(x => x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == requestId);
                //compare record version stored in session with record version of database
                if (recordVersion == serviceRequest.RecordVersion.Value.ToString() && serviceRequest.Status==Constants.SERVICE_PENDING)
                {
                    //if both are same then go ahead and check time conflicts
                    //fetch all the request with same service provider id and not yet completed
                    IEnumerable<ServiceRequest> otherUpcomingRequests = helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == spId && x.Status == Constants.SERVICE_ACCEPTED).ToList();
                    if (otherUpcomingRequests.Count() > 0)
                    {
                        foreach (var request in otherUpcomingRequests)
                        {
                            //conditions for 1 hour time difference between 2 requests
                            if (request.ServiceStartDate.Date.Equals(serviceRequest.ServiceStartDate.Date))
                            {
                                TimeSpan thisRequestStartTime = request.ServiceStartDate.TimeOfDay;
                                TimeSpan thisRequestEndTime = request.ServiceStartDate.AddHours(request.ServiceHours).TimeOfDay;
                                TimeSpan currentRequestStartTime = serviceRequest.ServiceStartDate.TimeOfDay;
                                TimeSpan currentRequestEndTime = serviceRequest.ServiceStartDate.AddHours(serviceRequest.ServiceHours).TimeOfDay;


                                if (currentRequestStartTime > thisRequestEndTime)
                                {
                                    double td1 = currentRequestStartTime.Subtract(thisRequestEndTime).TotalHours;
                                    if (td1 < 1)
                                    {
                                        isConflict = true;
                                        conflictedServiceId = request.ServiceRequestId;
                                        break;
                                    }
                                }
                                if (currentRequestEndTime < thisRequestStartTime)
                                {
                                    double td2 = thisRequestStartTime.Subtract(currentRequestEndTime).TotalHours;
                                    if (td2 < 1)
                                    {
                                        isConflict = true;
                                        conflictedServiceId = request.ServiceRequestId;
                                        break;
                                    }
                                }
                                if (((currentRequestStartTime > thisRequestStartTime) && (currentRequestStartTime < thisRequestEndTime)) || ((currentRequestEndTime > thisRequestStartTime) && (currentRequestEndTime < thisRequestEndTime)))
                                {
                                    isConflict = true;
                                    conflictedServiceId = request.ServiceRequestId;
                                    break;
                                }
                            }
                        }
                    }
                    //if there is conflict then show error message
                    if (isConflict)
                    {
                        var message = "Another service request " + conflictedServiceId + " has already been assigned which has time overlap with this service request. You can’t pick this one!";
                        ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                        return PartialView(serviceRequest);
                    }
                    //if there is no conflicts then go ahead and accept the request
                    else
                    {
                        serviceRequest.ServiceProviderId = spId;
                        serviceRequest.Status = Constants.SERVICE_ACCEPTED;
                        serviceRequest.ModifiedBy = spId;
                        serviceRequest.ModifiedDate = DateTime.Now;
                        serviceRequest.RecordVersion = Guid.NewGuid();
                        helperlandContext.ServiceRequests.Update(serviceRequest);
                        helperlandContext.SaveChanges();
                        //notify all other service providers that this request is no more available via email
                        var emailList = helperlandContext.Users.Where(user => user.UserTypeId == Constants.SERVICE_PROVIDER && user.ZipCode == serviceRequest.ServiceRequestAddresses.ElementAt(0).PostalCode).Select(user => user.Email).ToList();
                        string subject = "Service request no more available";
                        string body = "Service Request " + serviceRequest.ServiceRequestId + " has already been accepted by someone and is no more available to you.";
                        EmailManager.SendEmail(emailList, subject, body);
                        var message = "Request assigned to you";
                        ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                        return PartialView(serviceRequest);
                    }
                }
                //if both record versions are not same then that means service request has been accepted by another service provider so just show error message to logged in service provider
                else
                {
                    var message = "This service request is no more available. It has been assigned to another provider.";
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return PartialView(serviceRequest);
                }
            }
        }

        public IActionResult UpcomingServices()
        {
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            //fetch all the request which are accepted by service provider and not yet completed
            IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Include(x => x.User).ThenInclude(x => x.UserAddresses.Where(x => x.IsDefault == true)).Where(x => x.Status == Constants.SERVICE_ACCEPTED&& x.ServiceProviderId==id).ToList();
            //return view with service list
            return View(serviceRequests);
        }

        public IActionResult ServiceSchedule()
        {
            return View();
        }

        public JsonResult GetRequests()
        {
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            var requestList = helperlandContext.ServiceRequests.Where(x=>x.ServiceProviderId == id && (x.Status == Constants.SERVICE_COMPLETED || x.Status == Constants.SERVICE_ACCEPTED)).ToList();
            List<Object> objects = new List<Object>();
            foreach (var request in requestList)
            {
                var color = "";
                if (request.Status == Constants.SERVICE_ACCEPTED)
                {
                    color = "#1d7a8c";
                }
                else 
                {
                    color = "#67b644";
                }
                var v = new { title = request.ServiceStartDate.ToString("HH:mm")+" - "+request.ServiceStartDate.AddHours(request.ServiceHours).ToString("HH:mm"), color = color,start=request.ServiceStartDate,id=request.ServiceRequestId};
                objects.Add(v);
            }
            return Json(new { data = objects });
        }

        public IActionResult ServiceHistory()
        {
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            //fetch all the request which are completed by service provider
            IEnumerable<ServiceRequest> serviceRequests = helperlandContext.ServiceRequests.Include(x => x.User).ThenInclude(x => x.UserAddresses.Where(x => x.IsDefault == true)).Where(x => x.Status == Constants.SERVICE_COMPLETED && x.ServiceProviderId == id).ToList();
            //return view with service list
            return View(serviceRequests);
        }

        public IActionResult MyRatings()
        {
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            //fetch list of all ratings provided by customers to logged in service provider
            IEnumerable<Rating> ratings = helperlandContext.Ratings.Include(x => x.ServiceRequest).Include(x => x.RatingFromNavigation).Where(x => x.RatingTo == id).ToList();
            //return view with service list
            return View(ratings);
        }

        public IActionResult BlockCustomer()
        {
            int spID = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            BlockCustomerViewModel model = new BlockCustomerViewModel();
            //fetch list of all the blocked customers for whom service provider had worked
            model.allCustomers = helperlandContext.ServiceRequests.Include(x => x.User).Where(x => x.Status == Constants.SERVICE_COMPLETED && x.ServiceProviderId == spID).Select(x => x.User).Distinct().AsEnumerable();
            model.blockedCustomers =helperlandContext.FavoriteAndBlockeds.Include(x=>x.TargetUser).Where(x=>x.UserId==spID&&x.IsBlocked==true).Select(x=>x.TargetUser).ToList();
            //return view with service list
            return View(model);
        }

        [HttpPost]
        public JsonResult blockCustomer(int customerId) 
        {
            //block particular customer
            int spID = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked favoriteAndBlocked = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x=>x.UserId==spID && x.TargetUserId==customerId);
            if (favoriteAndBlocked == null)
            {
                favoriteAndBlocked = new FavoriteAndBlocked();
                favoriteAndBlocked.TargetUserId = customerId;
                favoriteAndBlocked.UserId = spID;
                favoriteAndBlocked.IsBlocked = true;
                favoriteAndBlocked.IsFavorite = false;
                helperlandContext.FavoriteAndBlockeds.Add(favoriteAndBlocked);
                helperlandContext.SaveChanges();
                return Json("ok");
            }
            else 
            {
                favoriteAndBlocked.IsBlocked = true;
                favoriteAndBlocked.IsFavorite = false;
                helperlandContext.FavoriteAndBlockeds.Update(favoriteAndBlocked);
                helperlandContext.SaveChanges();
                return Json("ok");
            }
        }

        [HttpPost]
        public JsonResult unblockCustomer(int customerId)
        {
            //unblock particular customer
            int spID = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            FavoriteAndBlocked favoriteAndBlocked = helperlandContext.FavoriteAndBlockeds.FirstOrDefault(x => x.UserId == spID && x.TargetUserId == customerId);
            favoriteAndBlocked.IsBlocked = false;
            favoriteAndBlocked.IsFavorite = true;
            helperlandContext.FavoriteAndBlockeds.Update(favoriteAndBlocked);
            helperlandContext.SaveChanges();
            return Json("ok");
        }

        public IActionResult MyAccount() 
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult MyDetails()
        {
            //get details of logged in service provider
            int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            User user = helperlandContext.Users.Include(x => x.UserAddresses.Where(x=>x.IsDefault==true)).FirstOrDefault(x=>x.UserId==id);
            MyDetailsViewModel model = new MyDetailsViewModel();
            model.IsActive = user.IsActive;
            model.UserProfilePicture = user.UserProfilePicture;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.Mobile = user.Mobile;
            model.Nationality = user.NationalityId;
            model.Gender = user.Gender;
            if (user.DateOfBirth != null)
            {
                var DOB = user.DateOfBirth.Value.ToString("dd/MMMM/yyyy").Split("-");
                model.BirthDay = DOB[0];
                model.BirthMonth = DOB[1];
                model.BirthYear = DOB[2];
            }
            if (user.UserAddresses.Count == 1)
            {
                model.AddressId = user.UserAddresses.ElementAt(0).AddressId;
                model.StreetName = user.UserAddresses.ElementAt(0).AddressLine1;
                model.HouseNumber = user.UserAddresses.ElementAt(0).AddressLine2;
                model.City = user.UserAddresses.ElementAt(0).City;
                model.PostalCode = user.ZipCode;
            }
            //return partialview with all the details
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult MyDetails(MyDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                //edit details of logged in service provider
                int id = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                User user = helperlandContext.Users.Include(x => x.UserAddresses.Where(x => x.IsDefault == true)).FirstOrDefault(x => x.UserId == id);
                string DOB = model.BirthDay + "-" + model.BirthMonth + "-" + model.BirthYear;
                user.DateOfBirth = DateTime.Parse(DOB);
                user.UserProfilePicture = model.UserProfilePicture;
                user.FirstName =model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.NationalityId = model.Nationality;
                user.Gender = model.Gender;
                if (user.UserAddresses.Count == 1)
                {
                    user.UserAddresses.ElementAt(0).AddressLine1= model.StreetName;
                    user.UserAddresses.ElementAt(0).AddressLine2= model.HouseNumber;
                    user.UserAddresses.ElementAt(0).City= model.City;
                    user.UserAddresses.ElementAt(0).PostalCode = model.PostalCode;
                    user.ZipCode =model.PostalCode;
                }
                //update details in the database
                helperlandContext.Users.Update(user);
                helperlandContext.SaveChanges();
                user.UserAddresses = null;
                //update user details in session
                HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
                var message = "Details Updated successfully..";
                ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                return PartialView(model);
            }
            else 
            {
                return PartialView(model);
            }
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
                if (user.Password.Equals(model.CurrentPassword))
                {
                    user.Password = model.ConfirmPassword;
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
