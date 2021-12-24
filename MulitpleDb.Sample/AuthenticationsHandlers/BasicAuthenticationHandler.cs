using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MulitpleDb.Sample.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.AuthenticationsHandlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly AuthenticationConfig _authenticationConfig;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AuthenticationConfig> authenticationConfig)
            : base(options, logger, encoder, clock)
        {
            _authenticationConfig = authenticationConfig.Value;
        }
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Basic";
            return base.HandleChallengeAsync(properties);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated)
                return await Task.FromResult(AuthenticateResult.NoResult());

            if (!string.IsNullOrWhiteSpace(Request.HttpContext.User?.Identity?.Name))
                return await Task.FromResult(AuthenticateResult.NoResult()); //Already authenticated


            if (!Request.Headers.ContainsKey("Authorization"))
                return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            string username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                if (!username.Equals(_authenticationConfig.Username) || !password.Equals(_authenticationConfig.Password))
                    throw new ArgumentException("Invalid username or password");
            }
            catch
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }



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
