using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Auth;
using Chattland.Api.DataAccess.Auth.Entities;
using Chattland.Api.DataAccess.DemoData;
using Chattland.Api.DataAccess.Entities;
using Chattland.Api.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

app.UseHttpsRedirection();

app.MapPost("/messages/{room}", async (IChatMessageRepository repo, ChatMessage message, string room) =>
{
	repo.SetCollectionName(room);
	await repo.AddOneAsync(message);
}).RequireAuthorization();

app.MapGet("/messages/{room}", async (IChatMessageRepository repo,  string room, int start, int count) =>
{
	repo.SetCollectionName(room);
	return await repo.GetManyAsync(start, count);
}).RequireAuthorization();

app.MapGet("/messages/rooms", async (IChatMessageRepository repo) =>
{
	return await repo.GetRoomNames();
}).RequireAuthorization();

app.MapHub<ChatHub>("/hubs/Chat").RequireAuthorization();

app.Run();
