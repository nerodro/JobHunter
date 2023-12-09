using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                  options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
              });
// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IUserLogic<>), typeof(UserLogic<>));
builder.Services.AddScoped(typeof(ICvLogic<>), typeof(CvLogic<>));
builder.Services.AddScoped(typeof(ILanguageLogic<>), typeof(LanguageLogic<>));
builder.Services.AddScoped(typeof(IRegistration<>), typeof(RegistrationLogic<>));
builder.Services.AddScoped(typeof(ILogin<>), typeof(LoginLogic<>));

builder.Services.AddSingleton<IConnection>(factory =>
{
    var rabbitMqFactory = new ConnectionFactory() { HostName = "localhost" };
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


builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IUserService, UserServices>();
builder.Services.AddTransient<ICvService, CvService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CategoryRpc>();
app.MapGrpcService<LocationRpc>();

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
