using Microsoft.AspNetCore.Mvc;
using SendsmsWeb.Contacts;
using SendsmsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Controllers
{
    public class ContactsController : Controller
    {
        public async Task<IActionResult> Index() => View(new ContactsViewModel
        {
            Contacts = string.IsNullOrWhiteSpace(Request.Query["tag"]) switch
            {
                true => await ContactsDbInterface.GetAllContacts(),
                false => await ContactsDbInterface.GetContactsFromTags(new List<string> { Request.Query["tag"] }, true)
            },
            Tags = await ContactsDbInterface.GetAllTags()
        });

        public IActionResult BuildContact()
        {
            throw new NotImplementedException();
        }
    }
}
