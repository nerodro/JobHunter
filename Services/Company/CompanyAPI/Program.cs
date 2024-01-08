using CompanyAPI.CompanyGrpc;
using CompanyAPI.RabbitMq;
using CompanyAPI.ServiceGrpc;
using CompanyRepository;
using CompanyRepository.CompanyLogic;
using CompanyRepository.Login;
using CompanyRepository.Registration;
using CompanyService.CompanyService;
using CompanyService.LoginService;
using CompanyService.RegistrationService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<CompanyContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(ICompanyLogic<>), typeof(CompanyLogic<>));
builder.Services.AddTransient<ICompanyService, CompanyServices>();
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

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ICompanyProducer, CompanyProducer>();

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
builder.Services.AddScoped<CategoryRpc>();
builder.Services.AddScoped<LocationRpc>();
builder.Services.AddGrpc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
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

var app = builder.Build();

app.MapGrpcService<CategoryRpc>();
app.MapGrpcService<LocationRpc>();
app.MapGrpcService<CompanyRpc>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
