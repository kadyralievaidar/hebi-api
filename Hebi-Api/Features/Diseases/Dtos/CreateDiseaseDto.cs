namespace Hebi_Api.Features.Diseases.Dtos;

/// <summary>
///     Create disease dto
/// </summary>
public class CreateDiseaseDto
{
    /// <summary>
    ///     Name of disease
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Disease's description
    /// </summary>
    public string? Description { get; set; }
}
