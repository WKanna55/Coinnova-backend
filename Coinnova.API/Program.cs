using Coinnova.API.Extensions;
using Coinnova.API.Middlewares;
using Coinnova.Application.Common.Files;
using Coinnova.Application.Interfaces;
using Coinnova.Application.Mappings;
using Coinnova.Application.Services;
using Coinnova.Domain.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories;
using Coinnova.Infrastructure.Repositories.Base;
using Coinnova.Infrastructure.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddControllers();

// Cargar el archivo .env
Env.Load();

// Ahora accedemos a las variables de entorno
var DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_DBCONTEXT");

// Conexion con la bd
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(DATABASE_URL));

// JWT y Swagger configurados desde extensiones
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwt();

// añadir configuracion de Mapster
builder.Services.AddMapster();



// Agregar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://coinnova-ts.vercel.app", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configuracion de Cloudinary
builder.Services.AddSingleton<ICloudStorageService, CloudinaryService>();

// ---------------------- inyeccion de repositorios y servicios ----------------------
// Repositorios
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IInstitutionRepository, InstitutionRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<IInstitutionEventRepository, InstitutionEventRepository>();

// Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IInstitutionService, InstitutionService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<FileUploadFactory>();

// -------------------------------- app construida --------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // uso de swagger
    app.UseSwagger();
    // extra de configuracion de swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger esté en "/"
        c.DefaultModelsExpandDepth(-1); // Ocultar modelos por defecto si no lo necesitas
    });
}

app.UseHttpsRedirection();



// agregar para que funcione
app.UseRouting();
app.UseAuthentication(); // agregado para jwt
app.UseCors("AllowFrontend");
app.UseAuthorization();


app.UseGlobalExceptionHandling(); // Excepciones 500 y más

//rate limit global
//app.MapControllers().RequireRateLimiting("GlobalFixedWindow"); // para swagger y APIRESTful
app.MapControllers();

app.UseCustomNotFound(); // Rutas no definidas (404)

// -------------------------------- Correr app --------------------------------
app.Run();
