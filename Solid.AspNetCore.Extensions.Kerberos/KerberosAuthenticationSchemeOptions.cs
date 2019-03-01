using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solid.AspNetCore.Extensions.Kerberos
{
    public class KerberosAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Spn { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
