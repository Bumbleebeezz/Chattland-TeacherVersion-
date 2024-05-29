using Microsoft.AspNetCore.Identity;

namespace Chattland.Api.DataAccess.Auth.Entities;

public class ChattlandUserRole : IdentityUserRole<string>
{
	public virtual ChattlandUser User { get; set; }
	public virtual ChattlandRole Role { get; set; }
}