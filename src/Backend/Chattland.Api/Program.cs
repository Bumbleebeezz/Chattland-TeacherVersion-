using Chattland.Api;
using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Auth;
using Chattland.Api.DataAccess.Auth.Entities;
using Chattland.Api.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddDbContext<UserDbContext>(
	options =>
	{
		options.UseInMemoryDatabase("ChattlandUserDb");
	});

builder.Services.AddDataAccess();

builder.Services.AddIdentityCore<ChattlandUser>()
	.AddEntityFrameworkStores<UserDbContext>()
	.AddApiEndpoints();

builder.Services
	.AddCors(options =>
		options.AddPolicy(
			"OmegalulChat",
			policy => policy.WithOrigins(["https://localhost:7053", "https://localhost:7194"])
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()));


builder.Services.AddSignalR();

var app = builder.Build();

app.MapIdentityApi<ChattlandUser>();

app.UseCors("OmegalulChat");

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/logout", async (SignInManager<ChattlandUser> signInManager, object empty) =>
{
	if (empty is not null)
	{
		await signInManager.SignOutAsync();

		return Results.Ok();
	}

	return Results.Unauthorized();
}).RequireAuthorization();

app.UseHttpsRedirection();

app.MapPost("/messages/{room}", async (IChatMessageRepository repo, ChatMessageDocument message, string room) =>
{
	repo.SetCollectionName(room);
	await repo.AddOneAsync(message);
});

app.MapGet("/messages/{room}", async (IChatMessageRepository repo,  string room, int start, int count) =>
{
	repo.SetCollectionName(room);
	return await repo.GetManyAsync(start, count);
});

app.MapGet("/messages/rooms", async (IChatMessageRepository repo) =>
{
	return await repo.GetRoomNames();
});


app.MapHub<ChatHub>("/hubs/ChatHub");

app.Run();