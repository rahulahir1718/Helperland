using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using HelperlandProject.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HelperlandProject.Controllers
{
    //only admins can access methods of this controller
    [Authorize(Roles = "3")]
    public class AdminController : Controller
    {
        private readonly HelperlandContext helperlandContext;
        public AdminController(HelperlandContext _helperlandContext)
        {
            helperlandContext = _helperlandContext;
        }
        public IActionResult ServiceRequests()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult ServiceRequestDetail(int id)
        {
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x => x.ServiceRequestAddresses).Include(x => x.ServiceRequestExtras).FirstOrDefault(x => x.ServiceRequestId == id);
            //returns partialview with service detail
            return PartialView(serviceRequest);
        }

        public IActionResult UserManagement()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserManagementData()
        {
            try 
            {
                //retrieve filter parameter from request
                IFormCollection form = HttpContext.Request.ReadFormAsync().Result;
                var set = form.ToHashSet();
                var draw = form["draw"].FirstOrDefault();
                var searchItems = form["search[value]"].FirstOrDefault().Length;
                var start = form["start"].FirstOrDefault(); ;
                var length = form["length"].FirstOrDefault();
                var sortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = form["order[0][dir]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt16(start) : 0;
                int recordsTotal = 0;

                int myId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                var data = helperlandContext.Users.Where(x=>x.IsDeleted!=true && x.UserId!=myId).ToList();
                //apply filters
                if (searchItems > 0)
                {
                    var searchArray = form["search[value]"].FirstOrDefault().Split(",");
                    var userName = searchArray[0];
                    var userType = searchArray[1];
                    var phoneNumber = searchArray[2];
                    var postalCode = searchArray[3];
                    var email = searchArray[4];
                    var fromDate = searchArray[5];
                    var toDate = searchArray[6];

                    if (!string.IsNullOrEmpty(userName))
                    {
                        var fullname = userName.Split(" ");
                        if (fullname.Length == 1)
                        {
                            char[] a = userName.ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            userName = new string(a);
                            data = data.Where(x => x.FirstName.Contains(userName) || x.LastName.Contains(userName)).ToList();
                        }
                        else
                        {
                            char[] a = fullname[0].ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            fullname[0] = new string(a);
                            a = fullname[1].ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            fullname[1] = new string(a);
                            data = data.Where(x => x.FirstName.Contains(fullname[0]) && x.LastName.Contains(fullname[1])).ToList();
                        }
                    }

                    if (!string.IsNullOrEmpty(userType))
                    {
                        data = data.Where(x => x.UserTypeId == Int16.Parse(userType)).ToList();
                    }

                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        data = data.Where(x => x.Mobile.Contains(phoneNumber)).ToList();
                    }

                    if (!string.IsNullOrEmpty(postalCode))
                    {
                        data = data.Where(x => x.ZipCode==postalCode).ToList();
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        data = data.Where(x => x.Email.Contains(email)).ToList();
                    }

                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        data = data.Where(x => x.CreatedDate >= DateTime.Parse(fromDate) && x.CreatedDate <= DateTime.Parse(toDate)).ToList();
                    }
                }
                //apply sorting on different columns
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    if (sortColumnDir.Equals("asc"))
                    {
                        switch (sortColumn)
                        {
                            case "User Name":
                                data = data.OrderBy(x => x.FirstName).ToList();
                                break;
                            case "Registration Date":
                                data = data.OrderBy(x => x.CreatedDate).ToList();
                                break;
                            case "Phone":
                                data = data.OrderBy(x => x.Mobile).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (sortColumn)
                        {
                            case "User Name":
                                data = data.OrderByDescending(x => x.FirstName).ToList();
                                break;
                            case "Registration Date":
                                data = data.OrderByDescending(x => x.CreatedDate).ToList();
                                break;
                            case "Phone":
                                data = data.OrderByDescending(x => x.Mobile).ToList();
                                break;
                        }
                    }
                }
                //return data according to page sige 
                recordsTotal = data.Count();
                data = data.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json("ok");
            }
        }

        public JsonResult Activate(int id)
        {
            //fetch particular user data and perform activate operation
            User user = helperlandContext.Users.FirstOrDefault(x => x.UserId == id);
            user.IsActive = true;
            user.IsApproved = true;
            user.Status = 1;
            user.ModifiedDate = DateTime.Now;
            helperlandContext.Users.Update(user);
            helperlandContext.SaveChanges();
            return Json("ok");
        }
        public JsonResult Deactivate(int id)
        {
            //fetch particular user data and perform deactivate operation
            User user = helperlandContext.Users.FirstOrDefault(x => x.UserId == id);
            user.IsActive = false;
            user.IsApproved = false;
            user.Status = 2;
            user.ModifiedDate = DateTime.Now;
            helperlandContext.Users.Update(user);
            helperlandContext.SaveChanges();
            return Json("ok");
        }

        public JsonResult Delete(int id) 
        {
            //fetch particular user data and perform delete operation
            User user = helperlandContext.Users.FirstOrDefault(x => x.UserId == id);
            user.IsDeleted = true;
            helperlandContext.Users.Update(user);
            helperlandContext.SaveChanges();
            return Json("ok");
        }

        [HttpPost]
        public JsonResult FilterData() 
        {
            try
            {
                //retrieve filter parameter from request
                IFormCollection form = HttpContext.Request.ReadFormAsync().Result;
                var set = form.ToHashSet();
                var draw = form["draw"].FirstOrDefault();
                var searchItems = form["search[value]"].FirstOrDefault().Length;
                var start = form["start"].FirstOrDefault(); ;
                var length = form["length"].FirstOrDefault();
                var sortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault()+ "][name]"].FirstOrDefault();
                var sortColumnDir = form["order[0][dir]"].FirstOrDefault();
                //var city = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt16(start) : 0;
                int recordsTotal = 0;

                var data = helperlandContext.ServiceRequests.Include(x => x.User).ThenInclude(x=>x.UserAddresses.Where(x=>x.IsDefault==true)).Include(x => x.ServiceProvider).ThenInclude(x => x.RatingRatingToNavigations).ToList();
                
                //apply filters
                if (searchItems > 0)
                {
                    var searchArray = form["search[value]"].FirstOrDefault().Split(",");
                    var serviceId = searchArray[0];
                    var postalCode = searchArray[1];
                    var email = searchArray[2];
                    var customerName = searchArray[3];
                    var spName = searchArray[4];
                    var status = searchArray[5];
                    var hasIssue = searchArray[6];
                    var fromDate = searchArray[7];
                    var toDate = searchArray[8];

                    if (!string.IsNullOrEmpty(serviceId))
                    {
                        data = data.Where(x => x.ServiceRequestId == Int16.Parse(serviceId)).ToList();
                    }

                    if (!string.IsNullOrEmpty(postalCode))
                    {
                        data = data.Where(x => x.ZipCode == postalCode).ToList();
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        data = data.Where(x => x.User.Email == email || x.ServiceProvider?.Email == email).ToList();
                    }

                    if (!string.IsNullOrEmpty(customerName))
                    {
                        var name = customerName.Split(" ");
                        if (name.Length == 1)
                        {
                            char[] a = customerName.ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            customerName = new string(a);
                            data = data.Where(x => x.User.FirstName.Contains(customerName) || x.User.LastName.Contains(customerName)).ToList();
                        }
                        else 
                        {
                            char[] a = name[0].ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            name[0] = new string(a);
                            a = name[1].ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            name[1] = new string(a);
                            data = data.Where(x => x.User.FirstName.Contains(name[0]) && x.User.LastName.Contains(name[1])).ToList();
                        }
                    }

                    if (!string.IsNullOrEmpty(spName))
                    {
                        var name = spName.Split(" ");
                        data = data.Where(x => x.ServiceProvider?.FirstName == name[0] && x.ServiceProvider.LastName == name[1]).ToList();

                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        data = data.Where(x => x.Status == Int16.Parse(status)).ToList();
                    }

                    data = data.Where(x => x.HasIssue ==bool.Parse(hasIssue)).ToList();

                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        data = data.Where(x => x.ServiceStartDate >= DateTime.Parse(fromDate) && x.ServiceStartDate <= DateTime.Parse(toDate)).ToList();
                    }
                }
                //apply sorting on different columns
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    if (sortColumnDir.Equals("asc"))
                    {
                        switch (sortColumn)
                        {
                            case "ServiceRequestId":
                                data = data.OrderBy(x => x.ServiceRequestId).ToList();
                                break;
                            case "ServiceStartDate":
                                data = data.OrderBy(x => x.ServiceStartDate).ToList();
                                break;
                            case "CustomerName":
                                data = data.OrderBy(x => x.User.FirstName).ToList();
                                break;
                            case "SPName":
                                data = data.OrderBy(x => x.ServiceProvider?.FirstName).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (sortColumn)
                        {
                            case "ServiceRequestId":
                                data = data.OrderByDescending(x => x.ServiceRequestId).ToList();
                                break;
                            case "ServiceStartDate":
                                data = data.OrderByDescending(x => x.ServiceStartDate).ToList();
                                break;
                            case "CustomerName":
                                data = data.OrderByDescending(x => x.User.FirstName).ToList();
                                break;
                            case "SPName":
                                data = data.OrderByDescending(x => x.ServiceProvider?.FirstName).ToList();
                                break;
                        }
                    }
                }
                //return data according to page size
                recordsTotal = data.Count();
                data = data.Skip(skip).Take(pageSize).ToList();
                List<AdminServiceRequest> adminServiceRequests=new List<AdminServiceRequest>();
                foreach (var request in data)
                {
                    var serviceRequest = new AdminServiceRequest();
                    serviceRequest.serviceRequestId = request.ServiceRequestId;
                    serviceRequest.serviceDate = request.ServiceStartDate.ToString("dd/MM/yyyy");
                    serviceRequest.serviceTime = request.ServiceStartDate.ToString("HH:mm") + "-" + request.ServiceStartDate.AddHours(request.ServiceHours).ToString("HH:mm");
                    serviceRequest.customerName = request.User.FirstName +" "+ request.User.LastName;
                    serviceRequest.customerAddress = request.User.UserAddresses.ElementAt(0).AddressLine1 +" "+ request.User.UserAddresses.ElementAt(0).AddressLine2 +"<br/>"+ request.User.UserAddresses.ElementAt(0).PostalCode +" "+ request.User.UserAddresses.ElementAt(0).City;
                    serviceRequest.spName = request.ServiceProvider?.FirstName + " "+ request.ServiceProvider?.LastName;
                    serviceRequest.spAvtar = request.ServiceProvider?.UserProfilePicture;
                    serviceRequest.spRating = request.ServiceProvider?.RatingRatingToNavigations.Average(x => x.Ratings);
                    serviceRequest.totalAmount = request.TotalCost.ToString();
                    serviceRequest.status = Constants.GetStatus(request.Status);
                    adminServiceRequests.Add(serviceRequest);
                }
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = adminServiceRequests });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return Json("ok");
            }
        }

        [HttpGet]
        public PartialViewResult EditRequest(int id)
        {
            //fetch particular request and return its data with partialview
            ServiceRequest? serviceRequest = helperlandContext.ServiceRequests.Include(x=>x.ServiceRequestAddresses).FirstOrDefault(x => x.ServiceRequestId == id);
            EditRequestViewModel editRequestViewModel = new EditRequestViewModel();
            editRequestViewModel.ServiceRequestId = serviceRequest.ServiceRequestId;
            editRequestViewModel.ServiceStartDate = serviceRequest.ServiceStartDate.Date;
            editRequestViewModel.ServiceStartTime = serviceRequest.ServiceStartDate.ToString("HH:mm:ss");
            editRequestViewModel.StreetName = serviceRequest.ServiceRequestAddresses.ElementAt(0).AddressLine1;
            editRequestViewModel.HouseNumber= serviceRequest.ServiceRequestAddresses.ElementAt(0).AddressLine2;
            editRequestViewModel.PostalCode= serviceRequest.ServiceRequestAddresses.ElementAt(0).PostalCode;
            editRequestViewModel.City= serviceRequest.ServiceRequestAddresses.ElementAt(0).City;
            return PartialView(editRequestViewModel);
        }

        [HttpPost]
        public PartialViewResult EditRequest(EditRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                string newDateString = model.ServiceStartDate.ToString("dd/MM/yyyy") + " " + model.ServiceStartTime;
                DateTime newDate = DateTime.Parse(newDateString);
                //fetch particular request and return its data with partialview
                ServiceRequest request = helperlandContext.ServiceRequests.Include(x => x.ServiceRequestAddresses).Include(x=>x.User).Include(x=>x.ServiceProvider).FirstOrDefault(x=>x.ServiceRequestId==model.ServiceRequestId);
                request.ServiceStartDate = newDate;
                request.ServiceRequestAddresses.ElementAt(0).AddressLine1 = model.StreetName;
                request.ServiceRequestAddresses.ElementAt(0).AddressLine2 = model.HouseNumber;
                request.ServiceRequestAddresses.ElementAt(0).PostalCode = model.PostalCode;
                request.ServiceRequestAddresses.ElementAt(0).City = model.City;
                if (model.RescheduleReason != null)
                {
                    request.Comments = model.RescheduleReason;
                }
                request.ModifiedDate = DateTime.Now;
                request.ModifiedBy= Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                helperlandContext.ServiceRequests.Update(request);
                helperlandContext.SaveChanges();
                //send email to user and service provider if exist
                List<string> emailList = new List<string>();
                emailList.Add(request.User.Email);
                if (request.ServiceProvider != null)
                {
                    emailList.Add(request.ServiceProvider.Email);
                }
                string Subject = "Admin has to inform you something !!";
                string Body = "Admin has reschedule the request " + request.ServiceRequestId + ", new date is " + newDate + ".";
                EmailManager.SendEmail(emailList,Subject,Body);
                var message = "You have successfully rescheduled the request.";
                ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                return PartialView(model);
            }
            else 
            {
                return PartialView(model);
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
            ServiceRequest serviceRequest = helperlandContext.ServiceRequests.Include(x=>x.User).Include(x => x.ServiceProvider).FirstOrDefault(x => x.ServiceRequestId == id);
            //set message for cancel request
            serviceRequest.Comments = comment;
            //set status to cancelled service=4
            serviceRequest.Status = Constants.SERVICE_CANCELLED;
            serviceRequest.ModifiedDate = DateTime.Now;
            helperlandContext.ServiceRequests.Update(serviceRequest);
            helperlandContext.SaveChanges();
            List<string> emailList = new List<string>();
            emailList.Add(serviceRequest.User.Email);
            //if request was accepted by any service provider then send email
            if (serviceRequest.ServiceProvider != null)
            {
                emailList.Add(serviceRequest.ServiceProvider.Email);
            }
            string subject = "Request Cancelled By Admin!";
            string body = "Service Request " + serviceRequest.ServiceRequestId + " has been cancelled by Admin.";
            EmailManager.SendEmail(emailList, subject, body);
            //return and show success message to customer
            var message = "Request cancelled successfully..";
            ViewBag.Alert = "<div class='alert alert-success alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            return PartialView();
        }
    }
}
