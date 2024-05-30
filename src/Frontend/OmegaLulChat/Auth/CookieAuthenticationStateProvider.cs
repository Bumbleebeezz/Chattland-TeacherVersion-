using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using OmegaLulChat.Auth.Models;

namespace OmegaLulChat.Auth;

public class CookieAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
{
	private readonly HttpClient _httpClient;

	private bool _authenticated = false;

	private readonly ClaimsPrincipal Unauthenticated =
		new(new ClaimsIdentity());

	private readonly JsonSerializerOptions jsonSerializerOptions =
		new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	public CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("chatApi");
	}

	public async Task<AuthResponseModel> RegisterAsync(string email, string password)
	{
		string[] defaultError = ["An unknown error prevented registration from succeeding."];

		try
		{
			var response = await _httpClient.PostAsJsonAsync("register",
				new
				{
					email,
					password
				});

			if (response.IsSuccessStatusCode)
			{
				return new AuthResponseModel() {Succeeded = true};
			}

			var errorTexts = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonDocument.Parse(errorTexts);
			var errors = new List<string>();

			var errorList = problemDetails.RootElement.GetProperty("errors");

			foreach (var error in errorList.EnumerateObject())
			{
				if (error.Value.ValueKind == JsonValueKind.String)
				{
					errors.Add(error.Value.GetString()!);
				}
				else if (error.Value.ValueKind == JsonValueKind.Array)
				{
					errors.AddRange(
						error.Value.EnumerateArray().Select(
								e => e.GetString() ?? string.Empty)
							.Where(e => !string.IsNullOrEmpty(e)));
				}
			}

			return new AuthResponseModel()
			{
				Succeeded = false,
				ErrorList = problemDetails is null ? defaultError : errors.ToArray()
			};
		}
		catch { }

		return new AuthResponseModel()
		{
			Succeeded = false,
			ErrorList = defaultError
		};
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		_authenticated = false;

		var user = Unauthenticated;

		try
		{
			var userResponse = await _httpClient.GetAsync("manage/info");

			userResponse.EnsureSuccessStatusCode();

			var userJson = await userResponse.Content.ReadAsStringAsync();
			var userInfo = JsonSerializer.Deserialize<UserModel>(userJson, jsonSerializerOptions);

			if (userInfo != null)
			{
				var claims = new List<Claim>
				{
					new(ClaimTypes.Name, userInfo.Email),
					new(ClaimTypes.Email, userInfo.Email)
				};

				//Todo: Här fick vi null ref exception då vi inte instansierade Claims i UserModel
				claims.AddRange(
					userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
						.Select(c => new Claim(c.Key, c.Value)));

				//Todo: Denna endpoint används för att hämta tillgängliga roller när claims skapas. Den saknades i vår kod.
				var rolesResponse = await _httpClient.GetAsync("roles");

				rolesResponse.EnsureSuccessStatusCode();

				var rolesJson = await rolesResponse.Content.ReadAsStringAsync();

				var roles = JsonSerializer.Deserialize<RoleClaim[]>(rolesJson, jsonSerializerOptions);

				if (roles?.Length > 0)
				{
					foreach (var role in roles)
					{
						if (!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
						{
							claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
						}
					}
				}

				var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
				user = new ClaimsPrincipal(id);
				_authenticated = true;
			}
		}
		catch { }

		return new AuthenticationState(user);
	}

	public async Task<AuthResponseModel> LoginAsync(string email, string password)
	{
		try
		{
			var result = await _httpClient.PostAsJsonAsync(
				"login?useCookies=true", new
				{
					email,
					password
				});

			if (result.IsSuccessStatusCode)
			{
				NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

				return new AuthResponseModel { Succeeded = true };
			}
		}
		catch { }

		return new AuthResponseModel
		{
			Succeeded = false,
			ErrorList = ["Invalid email and/or password."]
		};
	}

	public async Task LogoutAsync()
	{
		//Todo: Denna endpoint används för att logga ut användaren.
		const string Empty = "{}";
		var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");
		await _httpClient.PostAsync("logout", emptyContent);
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task<bool> CheckAuthenticatedAsync()
	{
		await GetAuthenticationStateAsync();
		return _authenticated;
	}

}