using Microsoft.AspNetCore.Identity;

namespace Chattland.Api.DataAccess.Auth.Entities;

public class ChattlandUser : IdentityUser
{
	public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
	public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
	public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }

	public virtual ICollection<ChattlandUserRole> UserRoles { get; set; }
}