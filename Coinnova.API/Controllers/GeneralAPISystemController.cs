using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[Route("/")]
public class GeneralAPISystemController : ControllerBase
{
    [HttpGet]
    public IActionResult GetWelcome() => Ok(new { message = "Bienvenido a la API de Coinnova." });

    [HttpGet("status")]
    public IActionResult GetStatus() => Ok(new { status = "OK", version = "1.0.0" });
    
    [HttpGet("info")]
    public IActionResult GetInfo() => Ok(new
        {
            name = "Coinnova API",
            description = "Plataforma para conectar comunidades, usuarios e instituciones",
            docs = "/swagger"
        });
    
}