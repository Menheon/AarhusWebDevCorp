using AarhusWebDevCoop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Net.Mail;
using Umbraco.Web.PublishedModels;
using Umbraco.Core;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        public ActionResult Index()
        {
            return PartialView("_ContactForm", new ContactForm());
        }
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            var mainMail = "kasper.d.borg@gmail.com";
            var mailMessage = new MailMessage();
            mailMessage.To.Add(mainMail);
            mailMessage.Subject = model.Subject;
            mailMessage.From = new MailAddress(model.Email, model.Name);
            mailMessage.Body = model.Message;

            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp-relay.sendinblue.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(
                    mainMail, "Whr6AxpcfFIbt45Y");
                smtp.Send(mailMessage);
            }
            var currentPageGuid = new GuidUdi(
                CurrentPage.ContentType.ItemType.ToString(),
                CurrentPage.Key
                );
            // Utilize Content API to create new Content
            var service = Services.ContentService;
            var contactSubmission = service.Create(
                model.Subject,
                currentPageGuid.Guid,
                ContactSubmission.ModelTypeAlias
            );
            var nameProp = ContactSubmission.GetModelPropertyType(c => c.MessageName);
            contactSubmission.SetValue(nameProp.Alias, model.Name);

            var contentProp = ContactSubmission.GetModelPropertyType(c => c.MessageContent);
            contactSubmission.SetValue(contentProp.Alias, model.Message);

            var emailProp = ContactSubmission.GetModelPropertyType(c => c.Email);
            contactSubmission.SetValue(emailProp.Alias, model.Email);

            var subjectProp = ContactSubmission.GetModelPropertyType(c => c.Subject);
            contactSubmission.SetValue(subjectProp.Alias, model.Subject);

            var naviHideProp = ContactSubmission.GetModelPropertyType(c => c.UmbracoNaviHide);
            contactSubmission.SetValue(naviHideProp.Alias, true);

            service.Save(contactSubmission);
            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}