using Kol1pop.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Kol1pop.Services;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ProjectResponseDTO?> GetProjectByIdAsync(int projectId)
    {
        var query = @"SELECT pp.ProjectId, pp.Objective, pp.StartDate, pp.EndDate,
                             a.Name, a.OriginDate,
                             i.InstitutionId, i.Name, i.FoundYear
                    FROM Preservation_Project pp
                    LEFT JOIN Artifact a ON pp.ArtifactId = a.ArtifactId
                    LEFT JOIN Institution i ON a.InstitutionId = i.InstitutionId
                    WHERE pp.ProjectId = @projectId;

                    SELECT s.FirstName, s.LastName, s.HireDate
                           sa.Role
                    FROM Staff_Assignment sa
                    INNER JOIN Staff s ON sa.StaffId = s.StaffId
                    WHERE sa.ProjectId = @projectId;";
        
        await using SqlConnection connection = 
            new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@projectId", projectId);
        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();

        ProjectResponseDTO? project = null;
        ArtifactResponseDTO? artifact = null;
        InstitutionResponseDTO? institution = null;

        if (!await reader.ReadAsync())
        {
            // SELECT nic nie znalazł
            throw new KeyNotFoundException("Brak projektu o takim id");
        }

        while (await reader.ReadAsync())
        {
            // jeśli to pierwszy wiersz, tworzymy obiekt klienta
            if (project is null)
            {
                project = new ProjectResponseDTO
                {
                    ProjectId = reader.GetInt32(0),
                    Objective = reader.GetString(1),
                    StartDate = reader.GetDateTime(2),
                    EndDate = reader.GetDateTime(3),
                    
                    Artifact = new ArtifactResponseDTO
                    {
                        Name = reader.GetString(4),
                        OriginDate = reader.GetDateTime(5),
                        
                        Institution = new InstitutionResponseDTO
                        {
                            InstitutionId = reader.GetInt32(6),
                            Name = reader.GetString(7),
                            FoundYear = reader.GetInt32(7),
                        }
                    },
                    
                    StaffAssigments = new List<StaffAssignmentDTO>()
                };
                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    project.StaffAssigments.Add(new StaffAssignmentDTO
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        HireDate = reader.GetDateTime(2),
                        Role = reader.GetString(3),
                    });
                }
            }
        }
        return project;
    }
}