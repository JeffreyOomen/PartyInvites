using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyInvites.Domain.Abstract;
using PartyInvites.Domain.Entities;
using PartyInvites.WebUI.Models;

namespace PartyInvites.WebUI.Controllers {
    public class HomeController : Controller {
        private IGuestRepository repository;
        private int pageSize = 3;

        public HomeController(IGuestRepository repository) {
            this.repository = repository;
        }

        public ViewResult Index() {
            return View();
        }

        [HttpGet]
        public ViewResult RsvpForm() {
            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guestResponse, HttpPostedFileBase image = null) {
            if (ModelState.IsValid) {
                if (image != null) {
                    guestResponse.ImageMimeType = image.ContentType;
                    guestResponse.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(guestResponse.ImageData, 0, image.ContentLength);
                }
                ViewBag.registerState = repository.AddGuest(guestResponse);
                return View("Thanks", guestResponse);
            } else {
                return View();
            }
        }

        public ViewResult GetGuests(string category = null, int page = 1) {
            GuestListViewModel model = new GuestListViewModel() {
                GuestResponses = repository.GuestResponses
                    .Where(g => category == null || g.WillAttend.ToString() == category)
                    .OrderBy(g => g.Name)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        repository.GuestResponses.Count() :
                        repository.GuestResponses.Count(g => g.WillAttend.ToString() == category)
                },
                CurrentCategory = category
            };

            if (model.GuestResponses.Any())
                return View("FilledGuestList", model);
            else
                return View("EmptyGuestList");
        }

        public PartialViewResult Widgets() {
            WidgetModel model = new WidgetModel() {
                PresentGuestCount = repository.GuestResponses
                    .Count(g => g.WillAttend == true),
                NotPresentGuestCount = repository.GuestResponses
                    .Count(g => g.WillAttend == false)
            };

            return PartialView(model);
        }

        public FileContentResult GetImage(string email) {
            GuestResponse guest = repository.GuestResponses
                .FirstOrDefault(g => g.Email == email);
            if (guest != null) {
                return File(guest.ImageData, guest.ImageMimeType);
            } else {
                return null;
            }
        }
    }
}