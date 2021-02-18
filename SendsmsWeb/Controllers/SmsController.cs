using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Controllers
{
    public class SmsController : Controller
    {
        public IActionResult BuildMessage() => View();

        [HttpPost]
        public async Task<IActionResult> SubmitMessage()
        {
            // Get params
            string Sender, Recipient, Message;
            try
            {
                Sender = !string.IsNullOrWhiteSpace(Request.Form["sender"]) ? Request.Form["sender"] : throw new Exception("Missing required parameter sender");
                Recipient = !string.IsNullOrWhiteSpace(Request.Form["recipient"]) ? Request.Form["recipient"] : throw new Exception("Missing required parameter recipient");
                Message = !string.IsNullOrWhiteSpace(Request.Form["message"]) ? Request.Form["message"] : throw new Exception("Missing required parameter message");
            }
            catch (Exception e)
            {
                ViewData["Title"] = "Error";
                return View("PlainTextView", e.Message);
            }

            // Send message
            string Result = await Program.TwilioSender.SendSms(Sender, Recipient, Message);

            return View("PlainTextView", $"Status: {Result}");
        }

    }
}
