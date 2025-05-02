# Proyecto Coinnova - Backend

Este proyecto implementa una arquitectura de **N capas** para mantener una separación clara de responsabilidades y mejorar la escalabilidad y el mantenimiento del sistema.

## Estructura de Capas

| Capa | Descripción |
| :--- | :--- |
| **API** | Capa de presentación. Expone endpoints HTTP a través de los Controllers. |
| **Application** | Orquesta los casos de uso y la lógica de negocio. Define los DTOs (Data Transfer Objects). |
| **Domain** | Define las entidades de negocio puras y los contratos de los repositorios (interfaces). |
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