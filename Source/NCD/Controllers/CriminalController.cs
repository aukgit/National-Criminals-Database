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
                    MaxNumberResults = model.MaxNumberResults.Value,
                    Name = model.Name,
                    AgeFrom = model.AgeFrom,
                    AgeTo = model.AgeTo,
                    HeightTo = model.HeightTo,
                    HeightFrom = model.HeightFrom,
                    WeightFrom = model.WeightFrom,
                    WeightTo = model.WeightTo
                };

                var criminals = SearchService.SearchCriminal(searchRequest);
                EmailService.Send(searchRequest.Email, criminals);

                return View("Confirmation");
            }

            return View("Index", model);
        }
    }
}