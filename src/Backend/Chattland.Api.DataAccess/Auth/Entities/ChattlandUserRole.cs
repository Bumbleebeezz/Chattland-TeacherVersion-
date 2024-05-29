using Chattland.Api.DataAccess.Auth.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.DataAccess.Entities;

public class ChattlandUserRole : IdentityUserRole<string>
{
    public virtual ChattlandUser User { get; set; }
    public virtual ChattlandRole Role { get; set; }
}
