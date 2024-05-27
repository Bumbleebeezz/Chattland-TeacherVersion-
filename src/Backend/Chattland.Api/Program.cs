using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Chattland.Api.Hubs;
using Chattland.DataTransferContract.ChatContracts;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess();

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

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("OmegaLulChat");

app.MapPost("/messages/{room}", async (IChatMessageRepository repo, ChatMessage message, string room) =>
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

app.MapHub<ChatHub>("/hubs/Chat");

app.Run();
