using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Solid.AspNetCore.Extensions.Kerberos;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SolidKerberosAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddKerberos(this AuthenticationBuilder builder, Action<KerberosAuthenticationSchemeOptions> configure) =>
            builder.AddKerberos(KerberosAuthenticationDefaults.AuthenticationScheme, configure);
        public static AuthenticationBuilder AddKerberos(this AuthenticationBuilder builder, string authenticationScheme, Action<KerberosAuthenticationSchemeOptions> configure)
        {
            builder.Services.TryAddSingleton<IPostConfigureOptions<KerberosAuthenticationSchemeOptions>, PostConfigureKerberosAuthenticationSchemeOptions>();
            builder.AddScheme<KerberosAuthenticationSchemeOptions, KerberosAuthenticationHandler>(authenticationScheme, configure);
            return builder;
        }
    }
}
