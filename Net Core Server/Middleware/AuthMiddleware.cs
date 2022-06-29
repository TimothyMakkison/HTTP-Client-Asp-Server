using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net_Core_Server.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Net_Core_Server.Middleware;

public class AuthMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UserContext userContext;

    public AuthMiddleware(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, UserContext userContext) : base(options, logger, encoder, clock)
    {
        this.userContext=userContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Request.Headers["ApiKey"].ToString();

        if (apiKey == "")
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var access = new UserDataAccess(userContext);
        var user = await access.TryGet(Guid.Parse(apiKey));

        if (user is null)
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
        };
        var identityClaim = new ClaimsIdentity(claims, "ApiKey");
        var claimsPrincipal = new ClaimsPrincipal(identityClaim);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}