namespace WindysLanding.Models
{
    public class ContactPageViewModel
    {
        public ContactFormViewModel ContactForm { get; set; } = new ContactFormViewModel();
        public NewsletterSignupViewModel NewsletterSignup { get; set; } = new NewsletterSignupViewModel();
    }
}