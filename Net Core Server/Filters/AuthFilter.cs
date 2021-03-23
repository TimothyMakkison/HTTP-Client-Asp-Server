using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Net_Core_Server.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = CheckRolePermission(context.HttpContext.User, Roles);

            if (!isAuthorized)
                context.Result = new UnauthorizedObjectResult("Unauthorized. Admin access only.");
        }

        private bool CheckRolePermission(ClaimsPrincipal claims, string condition)
        {
            if (Roles is null || Roles == "")
            {
                return true;
            }
            var role = claims.FindFirstValue(ClaimTypes.Role);
            return condition.Contains(role);
        }
    }
}