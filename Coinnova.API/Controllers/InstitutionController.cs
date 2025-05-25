using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles="admin")]
[Route("api/[controller]")]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionService _institutionService;

    public InstitutionController(IInstitutionService institutionService)
    {
        _institutionService = institutionService;
    }

    [HttpGet("all-summary")]
    public async Task<IActionResult> GetAllInstitutionsSummary()
    {
        var institutions = await _institutionService
            .GetAllInstitutionsSummary();
        
        return Ok(institutions);
    }

}