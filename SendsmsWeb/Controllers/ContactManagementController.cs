using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactManagementController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> DeleteContact()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact()
        {
            throw new NotImplementedException();
        }
    }
}
