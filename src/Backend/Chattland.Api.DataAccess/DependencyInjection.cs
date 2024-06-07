using Chattland.Api.DataAccess.Repositories;
using Chattland.Api.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chattland.Api.DataAccess;

public static class DependencyInjection
{
	public static IServiceCollection AddDataAccess(this IServiceCollection services)
	{
		services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

		
		return services;
	}
}