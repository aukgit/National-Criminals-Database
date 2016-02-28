using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCD.Application.Domain;
using NCD.Application.Services;
using NCD.Models;
using Ninject;

namespace NCD.Controllers {
    [Authorize]
    public class CriminalController : Controller {
        [Inject]
        public ISearchService SearchService { get; set; }

        [Inject]
        public IEmailService EmailService { get; set; }

        public ActionResult Index() {
            var model = new SearchViewModel {
                Email = HttpContext.GetOwinContext().Request.User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchViewModel model) {
            if (ModelState.IsValid) {
                var searchRequest = new SearchRequest {
                    Email = model.Email,
                    MaxNumberResults = model.MaxNumberResults.HasValue ? model.MaxNumberResults.Value : 0,
                    Name = model.Name,
                    AgeFrom = model.AgeFrom,
                    AgeTo = model.AgeTo,
                    HeightTo = model.HeightTo,
                    HeightFrom = model.HeightFrom,
                    WeightFrom = model.WeightFrom,
                    WeightTo = model.WeightTo
                };

                var criminals = SearchService.SearchCriminal(searchRequest);
                /** 
                 *  On a different, I could have used Any() on IEnumerable instead of changing the data type to IList.
                 *  Since the query already ran in the database and there is no new queries to add in future from C# then 
                 *  IList would be a better choice. 
                 *  
                 *  However, if there is any future chance of query the data from C# then IEnumerable<> would be a better choice.
                 *  Again, it depends on requirements.
                
                 *  for IEnumerable, Any() is faster and better
                 *  List Count is faster.
                 *  I know it is better to not change the method signature , 
                 *  however since we already executed the query it is better to have in memory object.
                 *  */
                if (criminals.Count > 0) {
                    var token = Guid.NewGuid();
                    string cacheToken = token.ToString();
                    var criminalRecordsViewModel = new CriminalRecordsViewModel() {
                        Criminals = criminals,
                        Token = token,
                        Email = searchRequest.Email
                    };

                    HttpContext.Cache[cacheToken] = criminalRecordsViewModel;
                    return View("Confirmation", criminalRecordsViewModel);
                } else {
                    ModelState.AddModelError("", "Sorry ! No results found with these parameters.");
                    return View("Index", model);
                }
            }
            ModelState.AddModelError("", "Sorry ! Invalid query parameters.");
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SendEmail(Guid? token) {
            if (token.HasValue) {
                var cacheToken = token.Value.ToString();
                var criminalRecords = HttpContext.Cache[cacheToken] as CriminalRecordsViewModel;
                if (criminalRecords != null) {
                    //send email to that given address.
                    EmailService.Send(criminalRecords.Email, criminalRecords.Criminals);
                    var result = new {
                        found = true,
                        message = "Email is sent successfully."
                    };
                    HttpContext.Cache.Remove(cacheToken);
                    // remove cache , don't need to keep it anymore.
                    return Json(result);
                }
            }
            var notFoundResult = new {
                found = false,
                message = "Email is not sent."
            };
            GC.Collect();
            return Json(notFoundResult);
        }
    }
}