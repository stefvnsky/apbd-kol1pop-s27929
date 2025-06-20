using Kol1pop.Models.DTOs;

namespace Kol1pop.Services;

public interface IDbService
{
    //GET
    Task <ProjectResponseDTO?> GetProjectByIdAsync(int projectId);
    
    //POST
    //Task AddNewArtifactAsync(ArtifactResponseDTO dto);
}