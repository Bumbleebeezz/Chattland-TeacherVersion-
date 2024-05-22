using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Chattland.DataTransferContract.ChatContracts;
using Microsoft.AspNetCore.Components.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess();

builder.Services
	.AddCors(options =>
		options.AddPolicy(
			"OmegalulChat",
			policy => policy.WithOrigins(["https://localhost:7053", "https://localhost:7194"])
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()));

var app = builder.Build();

app.UseCors("OmegalulChat");

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

app.Run();
