namespace Coinnova.Application.Common.Helpers;

/*
 * ¿Por qué en aca?
 * Aunque está relacionada con un proveedor externo (Cloudinary), no accede directamente
 * a la red ni a la infraestructura, sólo devuelve rutas (strings).
 *
 * ¿Que hace?
 * Define la estructura de carpetas en cloudinary
 */
public static class CloudinaryFolders
{
    public static string ForUser(int userId) => $"users/{userId}";
    public static string ForPost(int postId) => $"posts/{postId}";
    public static string ForEvent(int eventId) => $"events/{eventId}";
    public static string ForCommunity(int communityId) => $"communities/{communityId}";
    public static string ForInstitution(int institutionId) => $"institutions/{institutionId}";
}