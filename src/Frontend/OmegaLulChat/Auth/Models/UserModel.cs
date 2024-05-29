namespace OmegaLulChat.Auth.Models;

public class UserModel
{
	public string Email { get; set; }

	public bool IsEmailConfirmed { get; set; }

	public Dictionary<string, string> Claims { get; set; }
}