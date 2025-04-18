namespace Hebi_Api.Features.Users.Dtos;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public IEnumerable<string> Scopes { get; set; }
}
