using Microsoft.AspNetCore.Mvc;
using e_Prescription.Services;
using e_Prescription.Models.ViewModels;

namespace e_Prescription.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly EmailSender _emailSender;

        public EmailSenderController()
        {
            _emailSender = new EmailSender(); // Instantiate the EmailSender
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ToEmail))
            {
                TempData["ErrorMessage"] = "Recipient email is required.";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }

            if (string.IsNullOrWhiteSpace(model.Subject) || string.IsNullOrWhiteSpace(model.Message))
            {
                TempData["ErrorMessage"] = "Subject and Message are required.";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }

            try
            {
                // Use the EmailSender service to send the email
                _emailSender.SendMessage(model.ToEmail, model.Subject, model.Message);

                TempData["SuccessMessage"] = "Email sent successfully!";
                return RedirectToAction("SuccessView"); // Redirect to a success view
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while sending the email: {ex.Message}";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }
        }

        // Example of a ComposeEmail action to generate a form for sending emails
        [HttpGet]
        public IActionResult ComposeEmail(int admissionId)
        {
            var model = new EmailViewModel
            {
                AdmissionId = admissionId
            };

            // Return the view to compose an email (assuming you have a view set up)
            return View(model);
        }

        // Example of a SuccessView action
        public IActionResult SuccessView()
        {
            return View();
        }
    }
}
