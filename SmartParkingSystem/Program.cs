using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartParkingSystem.Infrastructure.Context;
using SmartParkingSystem.Infrastructure;
using System.Text;
using SmartParkingSystem.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Create JWT Token Configuration
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]);
var tokenValidationParemeters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
    ValidAudience = builder.Configuration["JwtConfig:Audience"]
};


builder.Services.AddSingleton(tokenValidationParemeters);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParemeters;
    jwt.RequireHttpsMetadata = false;
});

// Create connection string
string connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddValidator();
// Add core services
builder.Services.AddCoreServices();

// Add Infrastructure services
builder.Services.AddInfrastructureServices();

// Add maping
builder.Services.AddMapping();

// Add repositories
builder.Services.AddRepositories();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "FM Rozetka API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Parking System API");
        options.RoutePrefix = string.Empty; // To serve the Swagger UI at the root URL
    });
//}

app.UseCors(options =>
    //.WithOrigins(new[] {"https://localhost:3000"})
    options.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowCredentials().AllowAnyMethod()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
