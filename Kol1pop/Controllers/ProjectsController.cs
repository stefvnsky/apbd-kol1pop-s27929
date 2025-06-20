using Kol1pop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kol1pop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IDbService _service;

    public ProjectsController(IDbService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var project = await _service.GetProjectByIdAsync(id);
            if (project is null)
                return NotFound("Nie znaleziono projektu o podanym ID");
            return Ok(project);
        }
        catch (Exception e)
        {
             return StatusCode(500, $"Wystąpił błąd: {e.Message}");
        }
    }
}