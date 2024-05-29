namespace OmegaLulChat.Auth.Models;

public class AuthResponseModel
{
	public bool Succeeded { get; set; }
	public string[] ErrorList { get; set; } = [];
}