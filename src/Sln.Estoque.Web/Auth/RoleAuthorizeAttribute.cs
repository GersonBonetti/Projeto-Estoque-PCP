using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Sln.Estoque.Web.Auth
{
	public class RoleAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
			_role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
		{
			var user = context.HttpContext.User;

			if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _role))
			{
				context.Result = new ForbidResult();
			}
		}
	}
}