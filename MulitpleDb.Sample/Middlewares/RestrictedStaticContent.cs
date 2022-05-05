using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Middlewares
{
    public static class RestricteStaticContentExtensions
    {
        /// <summary>
        /// Restrict access to specific path
        /// </summary>
        /// <param name="builder">Application builder instance</param>
        /// <param name="path">Resource relative path</param>
        /// <returns></returns>
        public static IApplicationBuilder RestricteStaticContent(this IApplicationBuilder builder, PathString path)
        {
            return builder.UseMiddleware<RestricteStaticContent>(path);
        }

        /// <summary>
        /// Restrict access to specific path
        /// </summary>
        /// <param name="builder">Application builder instance</param>
        /// <param name="path">Resource relative path</param>
        /// <param name="policy">Authorization policy</param>
        /// <returns></returns>
        public static IApplicationBuilder RestricteStaticContent(this IApplicationBuilder builder, PathString path, string policy)
        {
            return builder.UseMiddleware<RestricteStaticContent>(path, policy);
        }
    }

    public class RestricteStaticContent
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _policyName;

        public RestricteStaticContent(RequestDelegate next, PathString path)
        {
            _next = next;
            _path = path;
        }

        public RestricteStaticContent(RequestDelegate next, PathString path, string policyName) : this(next, path)
        {
            _policyName = policyName;
        }

        public async Task Invoke(HttpContext httpContext,
                                 IAuthorizationService authorizationService)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                if (!string.IsNullOrEmpty(_policyName))
                {
                    var authorized = await authorizationService.AuthorizeAsync(httpContext.User, null, _policyName);
                    if (!authorized.Succeeded)
                    {
                        await AuthenticationHttpContextExtensions.ChallengeAsync(httpContext);
                        return;
                    }
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
