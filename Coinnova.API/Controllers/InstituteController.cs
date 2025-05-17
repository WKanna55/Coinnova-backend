using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles="admin")]
[Route("api/[controller]")]
public class InstituteController : ControllerBase
{

}