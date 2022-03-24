using HelperlandProject.Models;
using HelperlandProject.Models.Data;
using HelperlandProject.Models.ViewModels;
using HelperlandProject.Models.ViewModels.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HelperlandProject.Controllers
{
    //only customers can access methods of this controllersss
    [Authorize(Roles ="1")]
    public class BookServiceController : Controller
    {
        private readonly HelperlandContext helperlandContext;
        private IHttpContextAccessor httpContext;
        private User user;
        
        public BookServiceController(HelperlandContext _helperlandContext, IHttpContextAccessor _httpContext)
        {
            helperlandContext = _helperlandContext;
            httpContext = _httpContext;
            string currentUser = httpContext.HttpContext.Session.GetString("CurrentUser");
            user = JsonConvert.DeserializeObject<User>(currentUser);
        }

        public IActionResult CreateNewService()
        {
            ViewBag.Message = "Hello";
            ViewBag.ResultMessage = "Nothing";
            ViewBag.IsError = false;
            ViewBag.ServiceRequestId = 0;
            return View();
        }

        [HttpPost]
        public IActionResult ZipCode(ZipCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isServiceAvailable = helperlandContext.Users.Any(user => user.ZipCode == model.ZipCode && user.UserTypeId==2);
                if (isServiceAvailable)
                {
                    HttpContext.Session.SetString("ZipCodeViewModel",JsonConvert.SerializeObject(model));
                    ViewBag.Message= "ServiceSchedule";
                    return PartialView(model);
                }
                else
                {
                    var message = "We are not providing service in this area.<br/> We’ll notify you if any helper would start working near your area.";
                    ViewBag.Alert = "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + message + "<button type= 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    return View(model);
                }
            }
            else
            {
                return PartialView(model);
            }
        }

        [HttpPost]
        public IActionResult ServiceSchedule(ServiceScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var day= model.ServiceDate.ToString("dd-MM-yyyy");
                var time = model.ServiceTime.ToString("hh:mm:ss");
                var actual = day +" "+ time;
                DateTime dt = DateTime.Parse(actual);
                model.ServiceStartDate = dt;
                HttpContext.Session.SetString("ServiceScheduleViewModel", JsonConvert.SerializeObject(model));
                ViewBag.Message = "YourDetails";
                return PartialView(model);
            }
            else
            {
                return Json(ModelState.ValidationState);
            }
            
        }

        [HttpGet]
        public IActionResult YourDetails()
        {
            int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
            YourDetailsViewModel model = new YourDetailsViewModel();
            model.favouriteSP = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == customerId && x.IsFavorite == true && x.IsBlocked==false).Select(x => x.TargetUser).Distinct().ToList();
            model.userAddresses = helperlandContext.UserAddresses.Where(address => address.UserId == user.UserId && address.PostalCode == user.ZipCode).ToList();
            HttpContext.Session.SetString("UserAddressList", JsonConvert.SerializeObject(model.userAddresses));
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult YourDetails(YourDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                var value = HttpContext.Session.GetString("UserAddressList");
                model.userAddresses = JsonConvert.DeserializeObject<IEnumerable<UserAddress>>(value);
                HttpContext.Session.SetString("YourDetailsViewModel", JsonConvert.SerializeObject(model));
                model.favouriteSP = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == customerId && x.IsFavorite == true).Select(x => x.TargetUser).Distinct().ToList();
                ViewBag.Message = "MakePayment";
                return PartialView(model);
            }
            else 
            {
                return PartialView(model);
            }
            
        }

        [HttpPost]
        public IActionResult MakePayment([FromBody] ServiceRequest serviceRequest) 
        {
            var serviceScheduleViewModelString = HttpContext.Session.GetString("ServiceScheduleViewModel");
            ServiceScheduleViewModel serviceScheduleViewModel = JsonConvert.DeserializeObject<ServiceScheduleViewModel>(serviceScheduleViewModelString);
            var yourDetailsViewModelString = HttpContext.Session.GetString("YourDetailsViewModel");
            YourDetailsViewModel yourDetailsViewModel = JsonConvert.DeserializeObject<YourDetailsViewModel>(yourDetailsViewModelString);
            var zipCodeViewModelString = HttpContext.Session.GetString("ZipCodeViewModel");
            ZipCodeViewModel zipCodeViewModel = JsonConvert.DeserializeObject<ZipCodeViewModel>(zipCodeViewModelString);

            
            UserAddress userAddress = yourDetailsViewModel.userAddresses.FirstOrDefault(address=>address.AddressId==yourDetailsViewModel.check);
            int? spId = yourDetailsViewModel.selectedFSPId;
            serviceRequest.UserId = user.UserId;
            serviceRequest.ZipCode = zipCodeViewModel.ZipCode;
            serviceRequest.ServiceStartDate = serviceScheduleViewModel.ServiceStartDate;
            serviceRequest.ServiceHours = serviceScheduleViewModel.ServiceHours;
            serviceRequest.Comments = serviceScheduleViewModel.Comments;
            serviceRequest.HasPets = serviceScheduleViewModel.HasPets;
            serviceRequest.CreatedDate = DateTime.Now;
            serviceRequest.ModifiedDate = DateTime.Now;
            serviceRequest.ModifiedBy = user.UserId;
            serviceRequest.RecordVersion = Guid.NewGuid();
            serviceRequest.ServiceRequestAddresses.Add(new ServiceRequestAddress() { 
                AddressLine1=userAddress.AddressLine1,
                AddressLine2=userAddress.AddressLine2,
                City=userAddress.City,
                PostalCode=userAddress.PostalCode,
                Mobile=userAddress.Mobile,
                Email=userAddress.Email
            });
            if (spId == null || spId == 0)
            {
                serviceRequest.Status = Constants.SERVICE_PENDING;
                helperlandContext.ServiceRequests.Add(serviceRequest);
                var isError = helperlandContext.SaveChanges();
                int customerId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                //fetch all the block service providers id who have been blocked by logged in customer
                List<int> blockedSPIds = helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == customerId && x.IsBlocked == true).Select(x => x.TargetUserId).Distinct().ToList();
                var emailList = helperlandContext.Users.Where(user => user.UserTypeId == 2 && user.ZipCode == zipCodeViewModel.ZipCode && !blockedSPIds.Any(a => a == user.UserId)).Select(user => user.Email).ToList();
                string subject = "New Service Request arrived!!Hurry Up..";
                string body = "new service request arrived which is in your area and id of the service is " + serviceRequest.ServiceRequestId;
                EmailManager.SendEmail(emailList, subject, body);
            }
            else 
            {
                serviceRequest.Status = Constants.SERVICE_ACCEPTED;
                serviceRequest.ServiceProviderId = spId;
                helperlandContext.ServiceRequests.Add(serviceRequest);
                var isError = helperlandContext.SaveChanges();
                var emailList = helperlandContext.Users.Where(user => user.UserId==spId).Select(user => user.Email).ToList();
                string subject = "New Service Request arrived!!Hurry Up..";
                string body = "A service request "+serviceRequest.ServiceRequestId+" has been directly assigned to you.";
                EmailManager.SendEmail(emailList, subject, body);
            }
            ViewBag.IsError = false;
            ViewBag.ResultMessage = "Booking has been successfully submitted";
            ViewBag.ServiceRequestId = serviceRequest.ServiceRequestId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddNewAddress([FromBody] UserAddress address) 
        {
            address.Email = user.Email;
            address.UserId = user.UserId;
            address.State = "Gujarat";
            address.IsDefault = true;
            helperlandContext.UserAddresses.Add(address);
            helperlandContext.SaveChanges();
            return Json("Done..");
        }
    }
}
