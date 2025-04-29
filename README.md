# Proyecto Coinnova backend

Si quieres usar local 

Comando scaffold para el proyecto, ejecutar en la raiz de la solucion (Reemplazar los datos entre corchetes angulares <> con tus datos)
`dotnet ef dbcontext scaffold "Host=<myHost>;Port=<myPort-5432>;Database=<mydb>;Username=<myUsername>;Password=<myPassword>;" Npgsql.EntityFrameworkCore.PostgreSQL --project Coinnova.Infrastructure --output-dir Scaffold/Entities --context-dir Scaffold/Context --context ApplicationDbContext --no-pluralize --force`

para Neon, ejecutar en la raiz de la solucion 
`dotnet ef dbcontext scaffold "Host=<your-neon-host.com>;Database=<your_db_name>;Username=<your_username>;Password=<your_password>;Port=<5432>;SSL Mode=Require;Trust Server Certificate=true" Npgsql.EntityFrameworkCore.PostgreSQL --project Coinnova.Infrastructure --output-dir Scaffold/Entities --context-dir Scaffold/Context --context ApplicationDbContext --no-pluralize --force --schema public`

Dependencias del proyecto
1. Swagger
2. EntityFrameworkCore.Design
3. EntityFrameworkCore.Tools
