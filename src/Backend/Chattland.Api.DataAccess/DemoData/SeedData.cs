using Chattland.Api.DataAccess.Auth;
using Chattland.Api.DataAccess.Auth.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chattland.Api.DataAccess.DemoData;

public class SeedData
{
	private static readonly IEnumerable<SeedUser> seedUsers =
	[
		new SeedUser()
		{
			Email = "admin@admin.admin",
			NormalizedEmail = "ADMIN@ADMIN.ADMIN",
			NormalizedUserName = "ADMIN@ADMIN.ADMIN",
			RoleList = [ "Administrator", "Manager" ],
			UserName = "admin@admin.admin"
		},
		new SeedUser()
		{
			Email = "user@user.user",
			NormalizedEmail = "USER@USER.USER",
			NormalizedUserName = "USER@USER.USER",
			RoleList = [ "User" ],
			UserName = "user@user.user"
		},
	];

	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		using var context = new ChattlandAuthDbContext(serviceProvider.GetRequiredService<DbContextOptions<ChattlandAuthDbContext>>());

		if (context.Users.Any())
		{
			return;
		}

		var userStore = new UserStore<ChattlandUser>(context);
		var password = new PasswordHasher<ChattlandUser>();

		using var roleManager = serviceProvider.GetRequiredService<RoleManager<ChattlandRole>>();

		string[] roles = ["Administrator", "Manager", "User"];

		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new ChattlandRole(role));
			}
		}

		using var userManager = serviceProvider.GetRequiredService<UserManager<ChattlandUser>>();

		foreach (var user in seedUsers)
		{
			var hashed = password.HashPassword(user, "Passw0rd!");
			user.PasswordHash = hashed;
			await userStore.CreateAsync(user);

			if (user.Email is not null)
			{
				var appUser = await userManager.FindByEmailAsync(user.Email);

				if (appUser is not null && user.RoleList is not null)
				{
					await userManager.AddToRolesAsync(appUser, user.RoleList);
				}
			}
		}
		
		await context.SaveChangesAsync();
	}

	private class SeedUser : ChattlandUser
	{
		public string[]? RoleList { get; set; }
	}
}