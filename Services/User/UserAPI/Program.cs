using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using UserAPI.RabbitMq;
using UserAPI.ServiceGrpc;
using UserRepository.CvLogic;
using UserRepository.LanguageLogic;
using UserRepository.Login;
using UserRepository.Registration;
using UserRepository.UserContext;
using UserRepository.UserLogic;
using UserService.CvService;
using UserService.LanguageService;
using UserService.LoginService;
using UserService.RegistrationService;
using UserService.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IUserLogic<>), typeof(UserLogic<>));
builder.Services.AddScoped(typeof(ICvLogic<>), typeof(CvLogic<>));
builder.Services.AddScoped(typeof(ILanguageLogic<>), typeof(LanguageLogic<>));
builder.Services.AddScoped(typeof(IRegistration<>), typeof(RegistrationLogic<>));
builder.Services.AddScoped(typeof(ILogin<>), typeof(LoginLogic<>));

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<IConnection>(provider =>
{
    var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
    var rabbitMqFactory = new ConnectionFactory
    {
        HostName = options.Hostname,
        UserName = options.Username,
        Password = options.Password
    };
    return rabbitMqFactory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(provider =>
{
    var connection = provider.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

builder.Services.AddScoped<IResponseProducer, ResponseProducer>();


builder.Services.AddScoped<CategoryRpc>();
builder.Services.AddScoped<LocationRpc>();
builder.Services
    .AddGrpcClient<LocationServiceGrpc.LocationServiceGrpcClient>(o =>
    {
        o.Address = new Uri(builder.Configuration["Grpc:LocationHttp"]!);
        o.Address = new Uri(builder.Configuration["Grpc:CategoryHttp"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        return handler;
    });

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IUserService, UserServices>();
builder.Services.AddTransient<ICvService, CvService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
});

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
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
});
app.MapGrpcService<CategoryRpc>();
app.MapGrpcService<LocationRpc>();
app.MapGrpcService<CvRpc>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
