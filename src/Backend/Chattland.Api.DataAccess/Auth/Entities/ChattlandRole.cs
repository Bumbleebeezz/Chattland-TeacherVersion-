using Microsoft.AspNetCore.Identity;

namespace Chattland.Api.DataAccess.Auth.Entities;

public class ChattlandRole : IdentityRole
{
	public ChattlandRole()
	{
		
	}

	public ChattlandRole(string roleName) : base(roleName)
	{

	}

	public virtual ICollection<ChattlandUserRole> UserRoles { get; set; }
}