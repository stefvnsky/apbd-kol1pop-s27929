namespace Kol1pop.Models.DTOs;

public class ArtifactResponseDTO
{
    public string Name { get; set; } = null!;
    public DateTime OriginDate { get; set; }

    public InstitutionResponseDTO Institution { get; set; } = null!;
}