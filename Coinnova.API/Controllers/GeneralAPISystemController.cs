using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[Route("/")]
public class GeneralAPISystemController : ControllerBase
{
    /// <summary>
    /// Mensaje de bienvenida a la API.
    /// </summary>
    /// <returns>Mensaje simple de bienvenida.</returns>
    /// <response code="200">Petición exitosa.</response>
    [HttpGet]
    public IActionResult GetWelcome() => Ok(new { message = "Bienvenido a la API de Coinnova." });

    /// <summary>
    /// Verifica el estado actual de la API.
    /// </summary>
    /// <returns>Estado "OK" y versión actual.</returns>
    /// <response code="200">La API está funcionando correctamente.</response>
    [HttpGet("status")]
    public IActionResult GetStatus() => Ok(new { status = "OK", version = "1.0.0" });
    
    /// <summary>
    /// Proporciona información general sobre la API.
    /// </summary>
    /// <returns>Nombre, descripción y enlace a la documentación.</returns>
    /// <response code="200">Información obtenida exitosamente.</response>
    [HttpGet("info")]
    public IActionResult GetInfo() => Ok(new
        {
            name = "Coinnova API",
            description = "Plataforma para conectar comunidades, usuarios e instituciones",
            docs = "/swagger"
        });
    
}