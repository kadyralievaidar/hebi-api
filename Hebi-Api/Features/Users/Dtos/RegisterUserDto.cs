namespace Hebi_Api.Features.Users.Dtos;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<string> Scopes { get; set; }
}
