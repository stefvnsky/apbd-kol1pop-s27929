namespace Kol1pop.Models.DTOs;

public class ProjectResponseDTO
{
    public int ProjectId { get; set; }
    public string Objective { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }      //moze byc null
    
    public ArtifactResponseDTO Artifact { get; set; } = null!;
    public List<StaffAssignmentDTO> StaffAssigments { get; set; } = new List<StaffAssignmentDTO>();
    
}