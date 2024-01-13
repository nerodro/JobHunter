using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using ResponseAPI.RabbitMq;
using ResponseRepository.ResponseLogic;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using UserRepository.Login;
using UserRepository.Registration;
using UserService.LoginService;
using UserService.RegistrationService;
using VacancieAPI.RabbitMq;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieRepository;
using VacancieRepository.ResponseLogic;
using VacancieRepository.VacancieLogic;
using VacancieService.ResponseService;
using VacancieService.VacancieService;
using VacancieService.VacancyService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<VacancyContext>(options => options.UseNpgsql(connection), ServiceLifetime.Singleton);

builder.Services.AddScoped(typeof(IVacancieLogic<>), typeof(VacancieLogic<>));
builder.Services.AddScoped(typeof(IResponseLogic<>), typeof(ResponseLogic<>));



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

builder.Services.AddScoped<IVacancieProducercs, VacancieProducer>();
builder.Services.AddScoped<IResponseProducer, ResponseProducer>();

builder.Services.AddSingleton<QueueListenerService>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var producer = scopedProvider.GetRequiredService<IVacancieProducercs>();

        var listenerService = new QueueListenerService(producer);
        return listenerService;
    }
});

builder.Services.AddSingleton<QueueListenerResponse>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var producer = scopedProvider.GetRequiredService<IResponseProducer>();

        var listenerService = new QueueListenerResponse(producer);
        return listenerService;
    }
});

builder.Services.AddSingleton<IHostedService>(provider =>
{
    var listenerService = provider.GetRequiredService<QueueListenerService>();
    return listenerService;
});
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var listenerService = provider.GetRequiredService<QueueListenerResponse>();
    return listenerService;
});


builder.Services.AddScoped<LocationRpc>();
builder.Services.AddScoped<CompanyRpc>();
builder.Services.AddScoped<CvRpc>();
builder.Services
    .AddGrpcClient<LocationServiceGrpc.LocationServiceGrpcClient>(o =>
    {
        o.Address = new Uri(builder.Configuration["Grpc:LocationHttp"]!);
        o.Address = new Uri(builder.Configuration["Grpc:CompanyHttp"]!);
        o.Address = new Uri(builder.Configuration["Grpc:CvHttp"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        return handler;
    });

builder.Services.AddScoped<IVacancieService, VacancyServices>();
builder.Services.AddTransient<IResponseService, ResponseService>();
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
});
builder.Services.AddGrpc();
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
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<LocationRpc>();
app.MapGrpcService<CompanyRpc>();
app.MapGrpcService<CvRpc>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
