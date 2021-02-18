using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SendsmsWeb
{
    public class TwilioSender
    {
        string ServiceSID { get; set; }
        /// <summary>
        /// Generates a new instance of TwilioSender.
        /// </summary>
        /// <param name="AuthToken">The Twilio API auth token.</param>
        /// <param name="AccountSID">The Twilio API account SID.</param>
        /// <param name="_ServiceSID">The Twilio API service SID.</param>
        public TwilioSender(string AuthToken, string AccountSID, string _ServiceSID)
        {
            ServiceSID = _ServiceSID;

            // Initialize Twilio client
            TwilioClient.Init(AccountSID, AuthToken);
        }

        /// <summary>
        /// Sends an SMS message using the Twilio messaging API.
        /// </summary>
        /// <param name="Sender">The sender of the SMS. Must be an E.164 formatted phone number, or an alphanumeric sender using A-Z, a-z, 0-9, and space. Max 11 characters. It cannot be only numbers.</param>
        /// <param name="Recipient">The recipient of the SMS. Must be an E.164 formatted phone number.</param>
        /// <param name="Body">The contents of the SMS message. Must be less than 16000 characters.</param>
        /// <returns></returns>
        public async Task<string> SendSms(string Sender, string Recipient, string Body)
        {
            // Create message options
            var MessageOptions = new CreateMessageOptions(
                new PhoneNumber(Recipient))
            {
                From = new PhoneNumber(Sender),
                MessagingServiceSid = ServiceSID,
                Body = Body
            };

            // Send message
            var Message = await MessageResource.CreateAsync(MessageOptions);

            // Return status
            return Message.Status.ToString();
        }

    }
}
