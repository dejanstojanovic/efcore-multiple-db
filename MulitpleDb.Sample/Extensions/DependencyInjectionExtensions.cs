using Microsoft.AspNetCore.Authentication;
using MulitpleDb.Sample.AuthenticationsHandlers;
using MulitpleDb.Sample.Constants;

namespace MulitpleDb.Sample.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static AuthenticationBuilder AddApiKeyAuthenticationSchema(this AuthenticationBuilder authentication)
        {
            authentication.AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>(AuthenticationSchemaNames.ApiKeyAuthentication, o => { });
            return authentication;
        }

        public static AuthenticationBuilder AddBasicAuthenticationSchema(this AuthenticationBuilder authentication)
        {
            authentication.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(AuthenticationSchemaNames.BasicAuthentication, o => { });
            return authentication;
        }
    }
}
