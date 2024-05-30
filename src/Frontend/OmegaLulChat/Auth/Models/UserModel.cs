namespace OmegaLulChat.Auth.Models;

public class UserModel
{
	public string Email { get; set; }

	public bool IsEmailConfirmed { get; set; }

	//Todo: Denna var den stora boven. den instansierades inte så när claims skulle läggas till så kastades ett null ref exception detta fångades av vår try/catch men syntes då inte
	public Dictionary<string, string> Claims { get; set; } = [];
}