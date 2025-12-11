using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Eyebek.Api.Middleware;
using Eyebek.Api.Services;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Infrastructure.Persistence;
using Eyebek.Infrastructure.Repositories;
using Eyebek.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repos
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// Servicios
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection["Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Auditoría
app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

// Seed de planes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await PlanSeeder.SeedAsync(db);
}

app.Run();
