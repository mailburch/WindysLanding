using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WindysLanding.Models;
using WindysLanding.Services;

namespace WindysLanding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewBag.ContactInfo = _context.ContactInfos.FirstOrDefault();

            var model = new ContactPageViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactPageViewModel model)
        {
            ViewBag.ContactInfo = _context.ContactInfos.FirstOrDefault();

            ModelState.Clear();
            TryValidateModel(model.ContactForm, nameof(ContactPageViewModel.ContactForm));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _emailService.SendContactEmail(model.ContactForm);
                TempData["SuccessMessage"] = "Your message has been sent successfully.";
                return RedirectToAction("Contact");
            }
            catch
            {
                ModelState.AddModelError("", "Sorry, there was a problem sending your message.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsletterSignup(ContactPageViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.NewsletterSignup, nameof(ContactPageViewModel.NewsletterSignup));

            if (!ModelState.IsValid)
            {
                TempData["NewsletterError"] = "Please enter a valid email address.";
                return RedirectToAction("Contact");
            }

            try
            {
                _emailService.SendNewsletterSignupEmail(model.NewsletterSignup);
                TempData["NewsletterSuccess"] = "You have successfully signed up for the newsletter.";
                return RedirectToAction("Contact");
            }
            catch
            {
                TempData["NewsletterError"] = "Sorry, there was a problem signing you up.";
                return RedirectToAction("Contact");
            }
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
