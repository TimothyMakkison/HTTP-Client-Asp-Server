using Microsoft.AspNetCore.Http;
using Net_Core_Server.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Net_Core_Server.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;

        public AuthMiddleware(RequestDelegate next) => this.next = next;

        public async Task InvokeAsync(HttpContext context, UserContext userContext)
        {
            var dict = context.Request.Headers;
            var apiKey = dict["ApiKey"].ToString();

            if (apiKey != "")
            {
                UserDataAccess access = new UserDataAccess(userContext);
                var user = await access.TryGet(Guid.Parse(apiKey));

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
                    var identityClaim = new ClaimsIdentity(claims, "ApiKey");

                    context.User.AddIdentity(identityClaim);
                }
            }

            await next(context);
        }
    }
}