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
                 * */
                if (criminals.Count > 0) {
                    /**
                     * for IEnumerable, Any() is faster and better
                     * List Count is faster.
                     * I know it is better to not change the method signature , 
                     * however since we already executed the query it is better to have in memory object.
                     * */
                    EmailService.Send(searchRequest.Email, criminals);
                    return View("Confirmation");
                } else {
                    ModelState.AddModelError("", "Sorry ! No results found with these parameters.");
                    return View("Index", model);
                }
            }
            ModelState.AddModelError("", "Sorry ! Invalid query parameters.");
            return View("Index", model);
        }
    }
}