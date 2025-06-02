namespace Coinnova.Application.Dtos.Auth;

/// <summary>
/// DTO para el inicio de sesión con Google.
/// </summary>
public class GoogleLoginDto
{
    /// <summary>
    /// Token de ID emitido por Google tras la autenticación del usuario.
    /// </summary>
    public string IdToken { get; set; } = string.Empty;
}