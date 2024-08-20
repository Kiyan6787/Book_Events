using Book_Events.Attributes;
using System.Security.Claims;

namespace Book_Events.Middleware
{
    public class AcessControlMiddleware
    {
        private readonly RequestDelegate _next;

        public AcessControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var accessControlAttributes = endpoint.Metadata.GetOrderedMetadata<AccessControlAttribute>();
                if (accessControlAttributes.Any())
                {
                    var user = context.User;
                    if (!user.Identity.IsAuthenticated)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }

                    var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
                    foreach (var attribute in accessControlAttributes)
                    {
                        if (!roles.Contains(attribute.RoleGroup))
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
