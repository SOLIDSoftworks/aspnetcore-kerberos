using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCore.Kerberos.Pages
{
    [Authorize(AuthenticationSchemes = "Kerberos")]
    public class NegotiateModel : PageModel
    {
        public async Task OnGet(string returnUrl)
        {
            await HttpContext.SignInAsync(User);
            Response.Redirect(returnUrl);
        }
    }
}