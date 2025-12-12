using System.Text;
using Eyebek.Api.Services;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Infrastructure.Persistence;
using Eyebek.Infrastructure.Repositories;
using Eyebek.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // 👈 IMPORTANTE para Swagger

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger + esquema de seguridad JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eyebek API",
        Version = "v1"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduce tu JWT con el formato: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios (Infrastructure)
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
// 👇 Ya no registramos IAuditRepository porque no hay auditorías

// Servicios de aplicación (Application)
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

// Servicio para generar JWT (Api)
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Configuración de JWT desde appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? "CLAVE_POR_DEFECTO_SUPER_SECRETA";
var jwtIssuer = jwtSection["Issuer"] ?? "Eyebek";
var jwtAudience = jwtSection["Audience"] ?? "Eyebek";

// Autenticación con JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// CORS para permitir el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true); // simple para el integrador
    });
});

var app = builder.Build();

// Crear esquema de BD y seed de planes (sin tumbar la app si falla)
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Aplica migraciones pendientes
    db.Database.Migrate();

    // Seed de planes
    PlanSeeder.Seed(db);
}
catch (Exception ex)
{
    Console.WriteLine(" Error al preparar la base de datos o seed de planes:");
    Console.WriteLine(ex.Message);
    if (ex.InnerException != null)
    {
        Console.WriteLine(ex.InnerException.Message);
    }
}

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Ya NO usamos middleware de auditoría
// app.UseAuditMiddleware();

app.MapControllers();

app.Run();
