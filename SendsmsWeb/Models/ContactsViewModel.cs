using SendsmsWeb.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Models
{
    public class ContactsViewModel
    {
        public List<Contact> Contacts { get; set; }
        public List<string> Tags { get; set; }
    }
}
