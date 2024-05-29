using Microsoft.AspNetCore.Components.Authorization;
using OmegaLulChat.Auth.Models;

namespace OmegaLulChat.Auth;

public abstract class BaseStateProvider : AuthenticationStateProvider, IAccountManagement
{
	protected readonly HttpClient _httpClient;
	protected BaseStateProvider(IHttpClientFactory factory, string apiName)
	{
		_httpClient = factory.CreateClient(apiName);
	}
	public async Task<AuthResponseModel> LoginAsync(string email, string password)
	{
		throw new NotImplementedException();
	}

	public abstract Task LogoutAsync();

	public async Task<AuthResponseModel> RegisterAsync(string email, string password)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> CheckAuthenticatedAsync()
	{
		throw new NotImplementedException();
	}
}