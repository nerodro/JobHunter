using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddAuthentication(p =>
{
    p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                 builder.Configuration.GetSection("Jwt:Token").Value!))
    };
}).AddCookie();

builder.Services.AddCors(options =>
{
    options.AddPolicy("POLICY", builders =>
    {
        builders
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin(); // –азрешить доступ из любого источника
    });
});

var app = builder.Build();

app.UseCors("POLICY");
await app.UseOcelot();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
