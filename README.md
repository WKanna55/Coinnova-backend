# Proyecto Coinnova - Backend

Este proyecto implementa una arquitectura de **N capas** para mantener una separación clara de responsabilidades y mejorar la escalabilidad y el mantenimiento del sistema.

## Estructura de Capas

| Capa | Descripción |
| :--- | :--- |
| **API** | Capa de presentación. Expone endpoints HTTP a través de los Controllers y Middlewares. |
| **Application** | Capa de logica de negocio. Orquesta los casos de uso y la lógica de negocio. Define los DTOs y mapeos(Mapster). |
| **Domain** | Capa de dominio(corazón del sistema - Define qué es el negocio y qué reglas tiene) Define las entidades de negocio puras y los contratos de los repositorios (interfaces). |
| **Infrastructure** | Implementa el acceso a datos y los repositorios. |

---

## Uso Local: Scaffold del Proyecto

Para generar el contexto y las entidades a partir de una base de datos existente, ejecuta el siguiente comando en la raíz de la solución.  
**Recuerda reemplazar** los valores entre corchetes `<>` con tus datos reales:

```bash
dotnet ef dbcontext scaffold "Host=<myHost>;Port=<myPort-5432>;Database=<mydb>;Username=<myUsername>;Password=<myPassword>;" Npgsql.EntityFrameworkCore.PostgreSQL --project Coinnova.Infrastructure --output-dir Scaffold/Entities --context-dir Scaffold/Context --context ApplicationDbContext --no-pluralize --force
```

## Uso para Neon, ejecutar en la raiz de la solucion 
``` bash
dotnet ef dbcontext scaffold "Host=<your-neon-host.com>;Database=<your_db_name>;Username=<your_username>;Password=<your_password>;Port=<5432>;SSL Mode=Require;Trust Server Certificate=true" Npgsql.EntityFrameworkCore.PostgreSQL --project Coinnova.Infrastructure --output-dir Scaffold/Entities --context-dir Scaffold/Context --context ApplicationDbContext --no-pluralize --force --schema public
```
---

## Dependencias del proyecto
1. Swagger
2. EntityFrameworkCore.Design
3. EntityFrameworkCore.Tools
4. Npgsql.EntityFrameworkCore.PostgreSQL
5. DotNetEnv
6. Mapster
7. AspNetCore.Authentication.JwtBearer
8. BCrypt.Net-Next
---

## Convencional commits para github

| Tipo | Descripción |
| :--- | :--- |
| **feat** | Nueva funcionalidad |
| **fix**	| Corrección de errores |
| **docs**	| Cambios en la documentación |
| **style**	| Cambios de formato (espacios, comas, etc.) |
| **refactor**	| Reestructuración de código sin cambios de funcionalidad |
| **test**	| Agregado o modificación de pruebas |
| **chore**	| Tareas que no afectan el código de producción |

### Ejemplo 

```
feat: añadir botón de login
fix(auth): corregir bug al refrescar token
docs(readme): actualizar instrucciones de instalación
refactor: simplificar lógica de validación
chore: actualizar dependencias
```

---

## Convenciones para manejar los repositorios (nombres)

Idealmente un repositorio maneja una entidad, no acciones o casos de uso, por ejemplo.

```
/.infrastructure
  /Repositories
    /Base
      Repository.cs
    UserRepository.cs
    PostRepository.cs
    CommentRepository.cs
    ...
```

---

## Convenciones para manejar los servicios (nombres)

Los servicios usan nombres que reflejen claramente qué hacen desde el punto de vista del dominio, por ejemplo.

```
/.Application
  /Services
    AuthService.cs ← Lógica de autenticación(asociada a varias entidades).
    CommentService.cs 
    PostService.cs
    UserService.cs ← Lógica asociada a una entidad.
    ...
```

---

## Convenciones para manejar dtos (estructura y nombres)

Los Dtos se organizan por entidad o funcion(CRUD) dentro de una carpeta en comun, por ejemplo.

```
/.Application
  /Dtos
    /User
      - UserGetDto.cs
      - UserPostDto.cs
      - UserPutDto.cs
    /Post
      - PostGetDto.cs
      - PostPostDto.cs
      - PostPutDto.cs
      ...
    /Auth
    ...
```

Cuando no se trabaja con entidades, si no con casos de uso(no una simple operacion CRUD) como puede ser la autenticacion(Auth) la convencion cambia a request y response, por ejemplo.

```
/.Application
  /Dtos
    /Auth
      - LoginRequestDto.cs
      - LoginResponseDto.cs
      - RegisterRequestDto.cs
      - RegisterResponseDto.cs
      - ChangePasswordRequestDto.cs
      - ChangePasswordReponseDto.cs
    /Post
      ...
      - CreatePostRequestDto.cs
      - CreatePostResponseDto.cs
      ...
```

### Distinción entre CRUD DTO y Caso de Uso DTO

#### DTOs para CRUD (más directos):

1. Propósito: Mapear directamente los datos entre la base de datos y las vistas.
2. Usos: Crear, leer, actualizar o eliminar registros de forma directa.
3. Ejemplo: PostGetDto que se usa para  obtener una entidad post.

#### DTOs para Casos de Uso (con lógica de negocio):

1. Propósito: Controlar la entrada y salida de datos que requieren reglas de negocio, validaciones o lógica adicional.
2. Usos: Operaciones que van más allá de simplemente interactuar con la base de datos (por ejemplo, autenticación, creación de publicaciones con validaciones, etc.).
3. Ejemplo: CreatePostRequestDto y CreatePostResponseDto, donde la solicitud puede incluir validaciones adicionales (como la longitud del contenido) y el resultado incluye información como la fecha de creación, un ID generado, etc.

---

## Correcto Uso de Controladores

Tomemos como ejemplo la funcion `GetPostsForUserFeedById`, este toma el id de un usuario para mostrarle los posts segun sus comunidades suscritas. En si lo que se devuelve son Posts pero son para un usuario especifico, ¿En que controlador se deberia poner?.

### ¿Cómo decidir si una acción va en un controlador u otro?
Usa esta regla:

```
“¿Qué entidad es el recurso principal que se está manipulando o devolviendo?”
```

* Si devuelves posts filtrados por usuario/comunidad, el recurso son posts → PostController.

* Si estás actualizando o mostrando datos del usuario, el recurso es usuario → UserController.

## Rutas anidadas y sub-recursos(RESTful)

REST también permite rutas anidadas cuando hay dependencia contextual clara:

* `/users/{id}/posts` → posts del usuario → aún puede ir a PostController si estás devolviendo PostDto.

* `/communities/{id}/posts` → posts de una comunidad.

Pero la acción debería residir **en el controlador del recurso que se devuelve, no el que da contexto.**

## Buenas prácticas extra
* Cada controlador debe usar un único servicio (caso de uso) (Clean Architecture).

* No mezclar lógica cruzada en los controladores. Por ejemplo, no poner lógica de comunidad en el UserController.