using Kerberos.NET;
using Kerberos.NET.Crypto;
using Kerberos.NET.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Kerberos
{
    public class KerberosAuthenticationHandler : AuthenticationHandler<KerberosAuthenticationSchemeOptions>
    {
        public KerberosAuthenticationHandler(
            IOptionsMonitor<KerberosAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        } 

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {           
            var authorization = Request.Headers["authorization"];
            if (authorization == StringValues.Empty) return AuthenticateResult.Fail("No authorization header");
            if (!IsNegotiate(authorization)) return AuthenticateResult.NoResult();

            var authenticator = CreateAuthenticator();

            // Kerberos.NET supports sending the ticket with the Negotiate prefix, so we don't need to do any string manipulation
            var identity = await authenticator.Authenticate(authorization);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers.Add("WWW-Authenticate", "Negotiate");
            Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        private KerberosAuthenticator CreateAuthenticator()
        {
            // This could be done with DI and a factory pattern, but we're keeping it simple here.
            var principalName = new PrincipalName(PrincipalNameType.NT_PRINCIPAL, Options.Domain, new[] { Options.UserName });
            var key = new KerberosKey(Options.Password, principalName, Options.Password);
            var validator = new KerberosValidator(key);            
            var authenticator = new KerberosAuthenticator(validator);
            return authenticator;
        }

        private bool IsNegotiate(string authorizationHeader) => authorizationHeader.StartsWith("negotiate ", StringComparison.OrdinalIgnoreCase);
    }
}
