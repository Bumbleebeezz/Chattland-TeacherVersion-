using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Auth;
using Chattland.Api.DataAccess.Auth.Entities;
using Chattland.Api.DataAccess.DemoData;
using Chattland.Api.DataAccess.Entities;
using Chattland.Api.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Chattland.DataTransferContract.DataTransferTypes;
using Chattland.Api.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddAuthentication(IdentityConstants.ApplicationScheme)
	.AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<ChattlandAuthDbContext>(
	options =>
		options.UseInMemoryDatabase("ChattAuthDb")
);


builder.Services.AddDataAccess();

builder.Services.AddIdentityCore<ChattlandUser>()
	.AddRoles<ChattlandRole>()
	.AddEntityFrameworkStores<ChattlandAuthDbContext>()
	.AddApiEndpoints();

builder.Services
	.AddCors(options =>
			options.AddPolicy(
				"OmegaLulChat",
				policy =>
					policy
						.WithOrigins("https://localhost:7194", "https://localhost:7053")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials()
				)
		);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
	// Seed the database
	await using var scope = app.Services.CreateAsyncScope();
	await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.MapIdentityApi<ChattlandUser>();

app.UseCors("OmegaLulChat");

app.UseAuthentication();
app.UseAuthorization();

//Todo: Denna endpoint används för att logga ut användaren.
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

//Todo: Denna endpoint används för att hämta tillgängliga roller när claims skapas.
app.MapGet("/roles", (ClaimsPrincipal user) =>
{
	if (user.Identity is not null && user.Identity.IsAuthenticated)
	{
		var identity = (ClaimsIdentity)user.Identity;
		var roles = identity.FindAll(identity.RoleClaimType)
			.Select(c =>
				new
				{
					c.Issuer,
					c.OriginalIssuer,
					c.Type,
					c.Value,
					c.ValueType
				});

		return TypedResults.Json(roles);
	}

	return Results.Unauthorized();
}).RequireAuthorization();

app.MapPost("/messages/{room}", async (IChatMessageRepository repo, ChatMessage message, string room) =>
{
	var newMessage = new ChatMessageDocument
	{
		Message = message.Message,
		Sender = message.Sender,
		CreatedAt = DateTime.Now
	};
	repo.SetCollectionName(room);
	await repo.AddOneAsync(newMessage);
}).RequireAuthorization(policyBuilder => policyBuilder.RequireRole("User"));

app.MapGet("/messages/{room}", async (IChatMessageRepository repo,  string room, int start, int count) =>
{
	repo.SetCollectionName(room);
	return await repo.GetManyAsync(start, count);
}).RequireAuthorization(policyBuilder => policyBuilder.RequireRole("User"));

app.MapGet("/messages/rooms", async (IChatMessageRepository repo) =>
{
	return await repo.GetRoomNames();
}).RequireAuthorization(policyBuilder => policyBuilder.RequireRole("User"));

//Todo: Lagt till Authorize för att kräva inloggning för att ansluta till chatten
app.MapHub<ChatHub>("/hubs/Chat").RequireAuthorization();

app.Run();
