using DotNetEnv;

namespace Coinnova.API.Settings;

public class RateLimitSettings
{
    public int PermitLimit { get; set; }
    public int QueueLimit { get; set; }
    public int WindowSeconds { get; set; }

    /*
     * Configuracion para obtener un rate limit global 
     */
    public void SetGlobalLimit()
    {
        // Cargar las variables de entorno del archivo .env
        Env.Load();
        // Asignar los valores
        PermitLimit = int.Parse(Env.GetString("RATE_LIMIT_PERMIT"));
        QueueLimit = int.Parse(Env.GetString("RATE_LIMIT_QUEUE"));
        WindowSeconds = int.Parse(Env.GetString("RATE_LIMIT_WINDOW"));
    }
    
    /*
     * Configuracion para obtener un rate limit especifico para logear
     */
    public void SetLoginLimit()
    {
        // Cargar las variables de entorno del archivo .env
        Env.Load();
        // Asignar los valores
        PermitLimit = int.Parse(Env.GetString("LOGIN_LIMIT_PERMIT"));
        QueueLimit = int.Parse(Env.GetString("LOGIN_LIMIT_QUEUE"));
        WindowSeconds = int.Parse(Env.GetString("LOGIN_LIMIT_WINDOW"));
    }
    
}