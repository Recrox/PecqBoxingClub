using RamDam.BackEnd.Core.Services;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RamDam.BackEnd.Core.Utilities
{
    public class CurrentContextMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext, 
            CurrentContext currentContext,
            IUserService userService)
        {
            
            
            currentContext.HttpContext = httpContext;

            if (httpContext.User != null && httpContext.User.Claims.Any())
            {
                var claimsDict = httpContext.User.Claims
                    .GroupBy(c => c.Type)
                    .ToDictionary(c => c.Key, c => c.Select(v => v));

                var subject = GetClaimValue(claimsDict, JwtClaimTypes.Subject);
                if (Guid.TryParse(subject, out var subIdGuid))
                {
                    currentContext.UserId = subIdGuid;
                    currentContext.User = await userService.GetUserByPrincipalAsync(httpContext.User);
                }
            }

            using (LogContext.PushProperty("UserId", currentContext.UserId))
            {
                await _next.Invoke(httpContext);
            }
        }

        private string GetClaimValue(Dictionary<string, IEnumerable<Claim>> claims, string type)
        {
            if (!claims.ContainsKey(type))
            {
                return null;
            }

            return claims[type].FirstOrDefault()?.Value;
        }
    }
}
