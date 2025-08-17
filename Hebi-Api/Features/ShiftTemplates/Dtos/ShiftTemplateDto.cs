namespace Hebi_Api.Features.ShiftTemplates.Dtos;

public class ShiftTemplateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
}
