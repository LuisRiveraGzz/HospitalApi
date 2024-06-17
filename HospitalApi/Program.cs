using HospitalApi.Helpers;
using HospitalApi.Models.Entities;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Conexion a DB
string? Db = builder.Configuration.GetConnectionString("DbConnectionString");

builder.Services.AddDbContext<WebsitosHospitalbdContext>(x =>
{
    x.UseMySql(Db, ServerVersion.AutoDetect(Db));
});
builder.Services.AddCors();
#endregion
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HospitalApi", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        x =>
        {
            var issuer = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer");
            var audience = builder.Configuration.GetSection("Jwt").GetValue<string>("Audience");
            var secret = builder.Configuration.GetSection("Jwt").GetValue<string>("Secret");
            x.TokenValidationParameters = new()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                ValidateLifetime = true
            };

        }
);

builder.Services.AddSignalR();

builder.Services.AddSingleton<JwtHelper>();

//builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<SalasRepository>();
builder.Services.AddTransient<Repository<Paciente>>();
builder.Services.AddTransient<UsuariosRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
