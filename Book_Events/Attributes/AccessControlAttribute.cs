using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Book_Events.Attributes
{
    public class AccessControlAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string RoleGroup { get; set; }

        public string view = "ErrorView";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(RoleGroup))
            {
                return;
            }
            else
            {
                var res = new ViewResult() { ViewName = view };
                context.Result = res;
            }
            return;
        }
    }
}
