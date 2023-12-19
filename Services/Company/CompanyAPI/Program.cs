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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                  options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
              });

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
        o.Address = new Uri(builder.Configuration["Grpc:LocationHttp"]);
        o.Address = new Uri(builder.Configuration["Grpc:CategoryHttp"]);
        //o.Address = new Uri("https://locationapi:443");
        //o.Address = new Uri("https://categoryapi:443");
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
