using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PartyInvites.Domain.Abstract;

namespace PartyInvites.Controllers {
    public class NavController : Controller {
        private IGuestRepository repository;

        public NavController(IGuestRepository repo) {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null) {

            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.GuestResponses
                                    .Select(x => x.WillAttend.ToString())
                                    .Distinct()
                                    .OrderBy(x => x);
            
            return PartialView(categories);
        }

    }
}