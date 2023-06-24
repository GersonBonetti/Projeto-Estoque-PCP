namespace Sln.Estoque.Web.Auth
{
	public class UserRoleService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserRoleService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetUserRole()
		{
			var claims = _httpContextAccessor.HttpContext?.User.Claims;

			if (claims != null)
			{
				List<string> roles = claims
					.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
					.Select(c => c.Value)
					.ToList();

				return roles.FirstOrDefault();
			}

			return null;
		}
	}
}
