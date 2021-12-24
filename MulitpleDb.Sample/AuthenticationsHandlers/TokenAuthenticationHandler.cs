using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MulitpleDb.Sample.Constants;
using MulitpleDb.Sample.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.AuthenticationsHandlers
{
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly AuthenticationConfig _authenticationConfig;
        public TokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AuthenticationConfig> authenticationConfig)
            : base(options, logger, encoder, clock)
        {
            _authenticationConfig = authenticationConfig.Value;
        }
        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated)
                return await Task.FromResult(AuthenticateResult.NoResult());

            var headerKeyValue = Request.Headers.SingleOrDefault(h => h.Key.Equals(HeaderKeyNames.ApiKeyAuthenticationKey, StringComparison.InvariantCultureIgnoreCase)).Value.SingleOrDefault();

            if(string.IsNullOrEmpty(headerKeyValue) || !headerKeyValue.Equals(_authenticationConfig.Key, StringComparison.InvariantCultureIgnoreCase))
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

            var claims = new Claim[] { new Claim(ClaimTypes.Name, _authenticationConfig.Username) };
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            var user = new GenericPrincipal(
               identity: claimsIdentity,
               roles: claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray()
               );
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Request.HttpContext.User = user;

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
