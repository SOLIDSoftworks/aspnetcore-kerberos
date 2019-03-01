using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solid.AspNetCore.Extensions.Kerberos
{
    internal class PostConfigureKerberosAuthenticationSchemeOptions : IPostConfigureOptions<KerberosAuthenticationSchemeOptions>
    {
        public void PostConfigure(string name, KerberosAuthenticationSchemeOptions options)
        {
            var exceptions = new List<Exception>();
            if (string.IsNullOrEmpty(options.Spn))
                exceptions.Add(new ArgumentException("Missing or invalid property", nameof(options.Spn)));
            if (string.IsNullOrEmpty(options.Domain))
                exceptions.Add(new ArgumentException("Missing or invalid property", nameof(options.Domain)));
            if (string.IsNullOrEmpty(options.UserName))
                exceptions.Add(new ArgumentException("Missing or invalid property", nameof(options.UserName)));
            if (string.IsNullOrEmpty(options.Password))
                exceptions.Add(new ArgumentException("Missing or invalid property", nameof(options.Password)));

            if (exceptions.Any())
                throw new AggregateException("One or more configuration exceptions occurred.", exceptions);
        }
    }
}
