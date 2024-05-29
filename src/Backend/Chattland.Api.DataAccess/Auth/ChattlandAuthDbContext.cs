using Chattland.Api.DataAccess.Auth.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chattland.Api.DataAccess.Auth;

public class ChattlandAuthDbContext 
	: IdentityDbContext<ChattlandUser, ChattlandRole, string,
	IdentityUserClaim<string>, ChattlandUserRole, IdentityUserLogin<string>,
	IdentityRoleClaim<string>, IdentityUserToken<string>>
{

	public ChattlandAuthDbContext(DbContextOptions<ChattlandAuthDbContext> options)
	: base(options)
	{
		
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<ChattlandUser>(b =>
		{
			// Each User can have many UserClaims
			b.HasMany(e => e.Claims)
				.WithOne()
				.HasForeignKey(uc => uc.UserId)
				.IsRequired();

			// Each User can have many UserLogins
			b.HasMany(e => e.Logins)
				.WithOne()
				.HasForeignKey(ul => ul.UserId)
				.IsRequired();

			// Each User can have many UserTokens
			b.HasMany(e => e.Tokens)
				.WithOne()
				.HasForeignKey(ut => ut.UserId)
				.IsRequired();

			b.HasMany(e => e.UserRoles)
				.WithOne(e => e.User)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
		});

		builder.Entity<ChattlandRole>(b =>
			b.HasMany(e => e.UserRoles)
				.WithOne(e => e.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired()
		);
	}
}