namespace Hebi_Api.Features.UserCards.Dtos;

public class CreateUserCardDto
{
    public Guid UserId { get; set; }
    public ICollection<Guid>? AppointmnetIds { get; set; } 
}
